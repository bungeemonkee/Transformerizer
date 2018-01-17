﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Transformerizer
{
    /// <summary>
    /// The base of any <see cref="ITransformer{TProduce,TConsume}"/>.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed.</typeparam>
    public abstract class TransformerBase<TProduce, TConsume> : ITransformer<TProduce, TConsume>
    {
        /// <summary>
        /// The default number of threads used for any <see cref="ITransformer{TProduce, TConsume}"/> when it has no dependent transformation.
        /// Any <see cref="ITransformer{TProduce, TConsume}"/> with a dependent transformation will default to the number of threads used by it's dependent transformation.
        /// Using this value is almost always sub-optimal.
        /// Do you own testing about what thread count best optimizes each step in your transform.
        /// </summary>
        public static readonly int DefaultThreadCount =
            Environment.ProcessorCount > 2
            ? Environment.ProcessorCount / 2
            : 1;

        private readonly ITransformer _dependentTransformer;
        private readonly BlockingProducerConsumer<TProduce> _produce;

        private bool hasStarted;
        private volatile bool hasError;
        private volatile int _completedThreads;

        /// <summary>
        /// See <see cref="ITransformer{TProduce,TConsume}.Consume"/>.
        /// </summary>
        public IProducerConsumerCollection<TConsume> Consume { get; private set; }

        /// <summary>
        /// See <see cref="ITransformer{TProduce,TConsume}.Produce"/>.
        /// </summary>
        public IProducerConsumerCollection<TProduce> Produce => _produce;

        /// <summary>
        /// See <see cref="ITransformer.ThreadCount"/>.
        /// </summary>
        public int ThreadCount { get; private set; }
        
        /// <summary>
        /// Process a single consumed item, adding any results to the <see cref="Produce"/> collection.
        /// </summary>
        /// <param name="consume">The item to consume, already removed from the <see cref="Consume"/> collection.</param>
        protected abstract void ProcessConsume(TConsume consume);

        /// <summary>
        /// Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IProducerConsumerCollection<TConsume> consume)
            : this(consume, null, DefaultThreadCount)
        {
        }

        /// <summary>
        /// Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IProducerConsumerCollection<TConsume> consume, int threads)
            : this(consume, null, threads)
        {
        }

        /// <summary>
        /// Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IProducerConsumerCollection<TConsume> consume, ITransformer dependentTransformer)
            : this(consume, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        /// <summary>
        /// Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IProducerConsumerCollection<TConsume> consume, ITransformer dependentTransformer, int threads)
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

            _produce = new BlockingProducerConsumer<TProduce>();
        }

        /// <summary>
        /// Finalizer. Forced disposal of this instance.
        /// </summary>
        ~TransformerBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// See <see cref="IDisposable.Dispose()"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        /// <param name="finalizing">True if called from <see cref="Dispose()"/>, false otherwise.</param>
        protected virtual void Dispose(bool finalizing)
        {
            // If we can dispose of the production collection we must
            var disposable = Produce as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            if (_dependentTransformer != null)
            {
                // If we can dispose of the dependent transformer we must
                _dependentTransformer.Dispose();
            }
            else
            {
                // If there is no dependent transformation then dispose of the consumer source
                disposable = Consume as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// See <see cref="ITransformer.ExecuteAsync()"/>.
        /// </summary>
        public Task ExecuteAsync()
        {
            // Make sure this transformation has not been started already
            if (hasStarted)
            {
                throw new InvalidOperationException("This transformation has already been started.");
            }

            // Record the start of this transformation
            hasStarted = true;

            // If there is a dependent transformer then start it
            var dependentTask = _dependentTransformer?.ExecuteAsync();

            // Create the task that we will complete when this transformation is complete
            var taskCompletionSource = new TaskCompletionSource<object>();

            // Create the arguments for the process function
            var args = new Tuple<TaskCompletionSource<object>, Task>(taskCompletionSource, dependentTask);

            // Start all the work items for this process
            for (int i = 0; i < ThreadCount; ++i)
            {
                ThreadPool.QueueUserWorkItem(Process, args);
            }

            // Return the wait handle
            return taskCompletionSource.Task;
        }

        private void Process(object state)
        {
            // Cast the input to the right object
            var args = (Tuple<TaskCompletionSource<object>, Task>)state;

            // Process all the input
            TConsume consume;
            while (!hasError && Consume.TryTake(out consume))
            {
                try
                {
                    ProcessConsume(consume);
                }
                catch (Exception e)
                {
                    // Let other threads know of the error
                    hasError = true;

                    // Inform the task of the exception
                    args.Item1.TrySetException(e);
                }
            }

            // Determine if this is the last thread to complete
            var completedThreads = Interlocked.Increment(ref _completedThreads);
            if (completedThreads < ThreadCount) return;

            // Complete the production collection
            _produce.CompleteAdding();

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
    }
}
