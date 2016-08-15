using System;
using System.Threading;
using System.Threading.Tasks;

namespace Transformerizer
{
    /// <summary>
    ///     The base of any <see cref="ITransformer{TProduce,TConsume}" />.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed.</typeparam>
    public abstract class TransformerBase<TProduce, TConsume> : ITransformer<TProduce, TConsume>
    {
        /// <summary>
        ///     The default number of threads used for any <see cref="ITransformer{TProduce, TConsume}" /> when it has no dependent
        ///     transformation.
        ///     Any <see cref="ITransformer{TProduce, TConsume}" /> with a dependent transformation will default to the number of
        ///     threads used by it's dependent transformation.
        ///     Using this value is almost always sub-optimal.
        ///     Do you own testing about what thread count best optimizes each step in your transform.
        /// </summary>
        public static readonly int DefaultThreadCount =
            Environment.ProcessorCount > 2
                ? Environment.ProcessorCount / 2
                : 1;

        private readonly ITransformer _dependentTransformer;
        private readonly IBlockingQueueReadCount<TConsume> _consumeWithCount;
        private volatile bool _hasError;
        private int _completedThreads;
        private bool _hasStarted;

        /// <summary>
        ///     See <see cref="ITransformer{TProduce,TConsume}.Consume" />.
        /// </summary>
        public IBlockingQueueRead<TConsume> Consume { get; }

        /// <summary>
        ///     See <see cref="ITransformer{TProduce,TConsume}.Produce" />.
        /// </summary>
        public IBlockingQueue<TProduce> Produce { get; }

        /// <summary>
        ///     See <see cref="ITransformer.ThreadCount" />.
        /// </summary>
        public int ThreadCount { get; }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IBlockingQueueRead<TConsume> consume)
            : this(consume, null, DefaultThreadCount)
        {
        }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IBlockingQueueRead<TConsume> consume, int threads)
            : this(consume, null, threads)
        {
        }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IBlockingQueueRead<TConsume> consume, ITransformer dependentTransformer)
            : this(consume, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IBlockingQueueRead<TConsume> consume, ITransformer dependentTransformer, int threads)
        {
            if (consume == null)
            {
                throw new ArgumentNullException(nameof(consume));
            }

            if (threads < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(threads));
            }

            Consume = consume;
            _dependentTransformer = dependentTransformer;
            ThreadCount = threads;

            _consumeWithCount = consume as IBlockingQueueReadCount<TConsume>;
            Produce = new BlockingQueue<TProduce>();
        }

        /// <summary>
        ///     See <see cref="IDisposable.Dispose()" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     See <see cref="ITransformer.ExecuteAsync()" />.
        /// </summary>
        public Task ExecuteAsync()
        {
            // Make sure this transformation has not been started already
            if (_hasStarted)
            {
                throw new InvalidOperationException("This transformation has already been started.");
            }

            // Record the start of this transformation
            _hasStarted = true;

            // If there is a dependent transformer then start it
            var dependentTask = _dependentTransformer?.ExecuteAsync();

            // Create the task that we will complete when this transformation is complete
            var taskCompletionSource = new TaskCompletionSource<object>();

            // Create the arguments for the process function
            var args = new Tuple<TaskCompletionSource<object>, Task>(taskCompletionSource, dependentTask);

            // Start all the work items for this process
            for (var i = 0; i < ThreadCount; ++i)
            {
                ThreadPool.QueueUserWorkItem(Process, args);
            }

            // Return the wait handle
            return taskCompletionSource.Task;
        }

        /// <summary>
        ///     Process a single consumed item, adding any results to the <see cref="Produce" /> collection.
        /// </summary>
        /// <param name="consume">The item to consume, already removed from the <see cref="Consume" /> collection.</param>
        protected abstract void ProcessConsume(TConsume consume);

        /// <summary>
        ///     Finalizer. Forced disposal of this instance.
        /// </summary>
        ~TransformerBase()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Disposes of this instance.
        /// </summary>
        /// <param name="finalizing">True if called from <see cref="Dispose()" />, false otherwise.</param>
        protected virtual void Dispose(bool finalizing)
        {
            // If we can dispose of the production collection we must
            var disposable = Produce as IDisposable;
            disposable?.Dispose();

            if (_dependentTransformer != null)
            {
                // If we can dispose of the dependent transformer we must
                _dependentTransformer.Dispose();
            }
            else
            {
                // If there is no dependent transformation then dispose of the consumer source
                disposable = Consume as IDisposable;
                disposable?.Dispose();
            }
        }

        private void Process(object state)
        {
            const int bufferSize = 4;

            // Cast the input to the right object
            var args = (Tuple<TaskCompletionSource<object>, Task>)state;

            // Thread-local buffer of items to process
            TConsume[] consume;

            // Process all the input
            while (!_hasError && GetBuffer(bufferSize, out consume))
            {
                foreach (var item in consume)
                {
                    try
                    {
                        ProcessConsume(item);
                    }
                    catch (Exception e)
                    {
                        // Let other threads know of the error
                        _hasError = true;

                        // Inform the task of the exception
                        args.Item1.TrySetException(e);
                    }
                }
            }

            // Determine if this is the last thread to complete
            var completedThreads = Interlocked.Increment(ref _completedThreads);
            if (completedThreads < ThreadCount) return;

            // Complete the production collection
            Produce.CompleteAdding();

            if (args.Item2 != null)
            {
                // Now wait for the producing task to complete
                // It should already be done but we need to collect any exceptions
                try
                {
                    args.Item2.Wait();
                }
                catch (Exception e)
                {
                    // There was an exception in a dependent task
                    args.Item1.TrySetException(e);
                }
            }

            // Notify the handle that this is done
            args.Item1.TrySetResult(null);
        }

        private bool GetBuffer(int size, out TConsume[] consume)
        {
            if (_consumeWithCount == null)
            {
                size = 1;
            }
            else
            {
                // If there aren't enough items to fill the local thread buffer then scale the buffer down
                var count = _consumeWithCount.Count;
                if (size > 1 && count < size * ThreadCount)
                {
                    size = count / ThreadCount;
                    if (size < 1)
                    {
                        size = 1;
                    }
                }
            }

            // Get the buffer
            return Consume.TryTake(size, out consume);
        }
    }
}