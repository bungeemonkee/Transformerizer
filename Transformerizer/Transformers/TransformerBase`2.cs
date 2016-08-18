using System;
using Transformerizer.Collections;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     The base of any <see cref="ITransformer{TProduce,TConsume}" />.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed.</typeparam>
    public abstract class TransformerBase<TProduce, TConsume> : TransformerBase<TConsume>, ITransformer<TProduce, TConsume>
    {
        /// <summary>
        ///     See <see cref="IProduceTransformer{TProduce}.Produce" />.
        /// </summary>
        public IBlockingQueue<TProduce> Produce { get; }

        /// <summary>
        ///     Create a TransformerBase.
        /// </summary>
        protected TransformerBase(IBlockingQueueRead<TConsume> consume, ITransformer dependentTransformer, int threads)
            : base(consume, dependentTransformer, threads)
        {
            Produce = new BlockingQueue<TProduce>();
        }

        /// <summary>
        ///     Disposes of this instance.
        /// </summary>
        protected override void Dispose(bool finalizing)
        {
            base.Dispose(finalizing);

            // If we can dispose of the production collection we must
            var disposable = Produce as IDisposable;
            disposable?.Dispose();
        }


        /// <summary>
        ///     See <see cref="TransformerBase.ProcessComplete()" />.
        /// </summary>
        protected override void ProcessComplete()
        {
            // Complete the production collection
            Produce.CompleteAdding();
        }
    }
}