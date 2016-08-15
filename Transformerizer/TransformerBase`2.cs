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
    public abstract class TransformerBase<TProduce, TConsume> : TransformerBase, ITransformer<TProduce, TConsume>
    {
        private readonly IBlockingQueueReadCount<TConsume> _consumeWithCount;
        private volatile bool _hasError;
        private int _completedThreads;

        /// <summary>
        ///     See <see cref="ITransformer{TProduce,TConsume}.Consume" />.
        /// </summary>
        public IBlockingQueueRead<TConsume> Consume { get; }

        /// <summary>
        ///     See <see cref="ITransformer{TProduce,TConsume}.Produce" />.
        /// </summary>
        public IBlockingQueue<TProduce> Produce { get; }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IBlockingQueueRead<TConsume> consume, ITransformer dependentTransformer, int threads)
            : base(dependentTransformer, threads)
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

            _consumeWithCount = consume as IBlockingQueueReadCount<TConsume>;
            Produce = new BlockingQueue<TProduce>();
        }

        /// <summary>
        ///     Process a single consumed item, adding any results to the <see cref="Produce" /> collection.
        /// </summary>
        /// <param name="consume">The item to consume, already removed from the <see cref="Consume" /> collection.</param>
        protected abstract void ProcessConsume(TConsume consume);

        /// <summary>
        ///     Disposes of this instance.
        /// </summary>
        /// <param name="finalizing">True if called from <see cref="Dispose()" />, false otherwise.</param>
        protected override void Dispose(bool finalizing)
        {
            base.Dispose(finalizing);

            // If we can dispose of the production collection we must
            var disposable = Produce as IDisposable;
            disposable?.Dispose();

            if (DependentTransformer == null) return;
            // If there is no dependent transformation then dispose of the consumer source
            disposable = Consume as IDisposable;
            disposable?.Dispose();
        }

        protected override void Process(object state)
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