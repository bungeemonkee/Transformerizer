using System;
using Transformerizer.Collections;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     The base of any <see cref="IConsumeTransformer{TConsume}" />.
    /// </summary>
    /// <typeparam name="TConsume">The type of items consumed.</typeparam>
    public abstract class TransformerBase<TConsume> : TransformerBase, IConsumeTransformer<TConsume>
    {
        private readonly IBlockingQueueReadCount<TConsume> _consumeWithCount;

        private volatile bool _hasError;

        /// <summary>
        ///     See <see cref="IConsumeTransformer{TConsume}.Consume" />.
        /// </summary>
        public IBlockingQueueRead<TConsume> Consume { get; }

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
        }

        /// <summary>
        ///     Process a single consumed item.
        /// </summary>
        /// <param name="consume">The item to consume, already removed from the <see cref="Consume" /> collection.</param>
        protected abstract void ProcessConsume(TConsume consume);


        /// <summary>
        ///     See <see cref="TransformerBase.Dispose(bool)" />.
        /// </summary>
        protected override void Dispose(bool finalizing)
        {
            base.Dispose(finalizing);

            if (DependentTransformer == null) return;
            // If there is no dependent transformation then dispose of the consumer source
            var disposable = Consume as IDisposable;
            disposable?.Dispose();
        }

        /// <summary>
        ///     See <see cref="TransformerBase.Process()" />.
        /// </summary>
        protected override void Process()
        {
            const int bufferSize = 4;

            try
            {
                // Thread-local buffer of items to process
                TConsume[] consume;

                // Process all the input
                while (!_hasError && GetBuffer(bufferSize, out consume))
                {
                    foreach (var item in consume)
                    {
                        ProcessConsume(item);
                    }
                }
            }
            catch
            {
                // Let other threads know of the error
                _hasError = true;

                // Bubble the exception up
                throw;
            }
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
                if (size > 1 && count < size*ThreadCount)
                {
                    size = count/ThreadCount;
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