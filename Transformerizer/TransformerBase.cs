using System;
using System.Threading;
using System.Threading.Tasks;

namespace Transformerizer
{
    public abstract class TransformerBase : ITransformer
    {
        private bool _hasStarted;

        /// <summary>
        /// The transformer before this one in the chain.
        /// </summary>
        protected readonly ITransformer DependentTransformer;

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

        /// <summary>
        ///     See <see cref="ITransformer.ThreadCount" />.
        /// </summary>
        public int ThreadCount { get; }

        /// <summary>
        /// See <see cref="ITransformer.PreserveNulls"/>.
        /// </summary>
        public bool PreserveNulls { get; set; }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(ITransformer dependentTransformer, int threads)
        {
            DependentTransformer = dependentTransformer;
            ThreadCount = threads;

            PreserveNulls = DependentTransformer?.PreserveNulls ?? false;
        }

        /// <summary>
        ///     Finalizer. Forced disposal of this instance.
        /// </summary>
        ~TransformerBase()
        {
            Dispose(false);
        }

        protected abstract void Process(object state);

        /// <summary>
        ///     See <see cref="ITransformer.ExecuteAsync()" />.
        /// </summary>
        public Task ExecuteAsync()
        {
            lock (this)
            {
                // Make sure this transformation has not been started already
                if (_hasStarted)
                {
                    throw new InvalidOperationException("This transformation has already been started.");
                }

                // Record the start of this transformation
                _hasStarted = true;
            }

            // If there is a dependent transformer then start it
            var dependentTask = DependentTransformer?.ExecuteAsync();

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
        ///     See <see cref="IDisposable.Dispose()" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes of this instance.
        /// </summary>
        /// <param name="finalizing">True if called from <see cref="Dispose()" />, false otherwise.</param>
        protected virtual void Dispose(bool finalizing)
        {
            // If we can dispose of the dependent transformer we must
            DependentTransformer?.Dispose();
        }
    }
}
