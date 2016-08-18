using Transformerizer.Collections;
using Transformerizer.Methods;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     An <see cref="IConsumeTransformer{TConsume}" /> that processes <see cref="TransformVoid{TConsume}" />
    ///     delegates.
    /// </summary>
    /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
    public class TransformerVoid<TConsume> : TransformerBase<TConsume>
    {
        private readonly TransformVoid<TConsume> _transform;

        /// <summary>
        ///     Creates a new TransformerVoid instance.
        /// </summary>
        public TransformerVoid(IBlockingQueueRead<TConsume> consume, TransformVoid<TConsume> transform)
            : this(consume, transform, null, DefaultThreadCount)
        {
        }

        /// <summary>
        ///     Creates a new TransformerVoid instance.
        /// </summary>
        public TransformerVoid(IBlockingQueueRead<TConsume> consume, TransformVoid<TConsume> transform, int threads)
            : this(consume, transform, null, threads)
        {
        }

        /// <summary>
        ///     Creates a new TransformerVoid instance.
        /// </summary>
        public TransformerVoid(IBlockingQueueRead<TConsume> consume, TransformVoid<TConsume> transform, ITransformer dependentTransformer)
            : this(consume, transform, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        /// <summary>
        ///     Creates a new TransformerVoid instance.
        /// </summary>
        public TransformerVoid(IBlockingQueueRead<TConsume> consume, TransformVoid<TConsume> transform, ITransformer dependentTransformer, int threads)
            : base(consume, dependentTransformer, threads)
        {
            _transform = transform;
        }

        /// <summary>
        /// See <see cref="TransformerBase.ProcessComplete()"/>.
        /// </summary>
        protected override void ProcessComplete()
        {
        }

        /// <summary>
        ///     See <see cref="TransformerBase{TConsume}.ProcessConsume(TConsume)" />.
        /// </summary>
        protected override void ProcessConsume(TConsume consume)
        {
            // Execute the transformation
            _transform(consume);
        }
    }
}