using System.Collections.Concurrent;

namespace Transformerizer
{
    /// <summary>
    ///     An <see cref="ITransformer{TProduce,TConsume}" /> that processes <see cref="Transform{TProduce,TConsume}" />
    ///     delegates.
    /// </summary>
    /// <typeparam name="TProduce">The type of the items produced.</typeparam>
    /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
    public class Transformer<TProduce, TConsume> : TransformerBase<TProduce, TConsume>
    {
        private readonly Transform<TProduce, TConsume> _transform;

        /// <summary>
        ///     Creates a new Transformer instance.
        /// </summary>
        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform)
            : this(consume, transform, null, DefaultThreadCount)
        {
        }

        /// <summary>
        ///     Creates a new Transformer instance.
        /// </summary>
        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform, int threads)
            : this(consume, transform, null, threads)
        {
        }

        /// <summary>
        ///     Creates a new Transformer instance.
        /// </summary>
        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform, ITransformer dependentTransformer)
            : this(consume, transform, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        /// <summary>
        ///     Creates a new Transformer instance.
        /// </summary>
        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform, ITransformer dependentTransformer, int threads)
            : base(consume, dependentTransformer, threads)
        {
            _transform = transform;
        }

        /// <summary>
        ///     See <see cref="TransformerBase{TProduce,TConsume}.ProcessConsume(TConsume)" />.
        /// </summary>
        protected override void ProcessConsume(TConsume consume)
        {
            var result = _transform(consume);
            Produce.TryAdd(result);
        }
    }
}