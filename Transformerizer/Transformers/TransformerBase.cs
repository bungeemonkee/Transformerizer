using System;
using System.Threading;
using System.Threading.Tasks;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     The base of any <see cref="ITransformer" />.
    /// </summary>
    public abstract class TransformerBase : ITransformer
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

        /// <summary>
        ///     The transformer before this one in the chain.
        /// </summary>
        protected readonly ITransformer DependentTransformer;

        private bool _hasStarted;
        private int _completedThreads;

        /// <summary>
        ///     See <see cref="ITransformer.ThreadCount" />.
        /// </summary>
        public int ThreadCount { get; }

        /// <summary>
        ///     See <see cref="ITransformer.PreserveNulls" />.
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

        private void Process(object state)
        {
            // Cast the input to the right object
            var args = (Tuple<TaskCompletionSource<object>, Task>)state;

            try
            {
                Process();
            }
            catch (Exception exception)
            {
                // Inform the task of the exception
                args.Item1.TrySetException(exception);
            }

            // Determine if this is the last thread to complete
            var completedThreads = Interlocked.Increment(ref _completedThreads);
            if (completedThreads < ThreadCount) return;

            // This is the last thread to complete so execute final processing
            ProcessComplete();

            if (args.Item2 != null)
            {
                // Now wait for the producing task to complete.
                // It should already be done but we need to collect any exceptions.
                // Do this instead of calling Wait() to prevent the stack trace for the exception from getting chopped in half and reset to this location in the code.
                // Put another way: We want to pass the original exception that aborted the task along unchanged rather than intercepting and wrapping it via try/catch.
                args.Item2.ContinueWith(ProcessContinue, args.Item1);
            }
            else
            {
                // Notify the handle that this is done
                args.Item1.TrySetResult(null);
            }
        }

        private static void ProcessContinue(Task innerTask, object outerTask)
        {
            var outerTaskSource = (TaskCompletionSource<object>) outerTask;

            if (innerTask.IsCanceled)
            {
                outerTaskSource.TrySetCanceled();
            }

            if (innerTask.IsFaulted)
            {
                outerTaskSource.TrySetException(innerTask.Exception);
            }

            outerTaskSource.TrySetResult(null);
        }

        /// <summary>
        ///     The main process method. Executed on every thread simultaneously. Must continue the transformation in a loop until
        ///     the transformation is complete.
        /// </summary>
        protected abstract void Process();

        /// <summary>
        ///     Complete processing on the last thread to finish.
        /// </summary>
        protected abstract void ProcessComplete();

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
                var thread = new Thread((ParameterizedThreadStart)Process)
                {
                    IsBackground = true,
                    Name = "Transformer Worker Thread"
                };
                thread.Start(args);
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