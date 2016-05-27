using System.Collections.Concurrent;

namespace Transformerizer
{
    public class TransformerMany<TProduce, TConsume> : TransformerBase<TProduce, TConsume>
    {
        private readonly TransformMany<TProduce, TConsume> _transform;

        public TransformerMany(IProducerConsumerCollection<TConsume> consume, TransformMany<TProduce, TConsume> transform)
            : this(consume, transform, null, DefaultThreadCount)
        {
        }

        public TransformerMany(IProducerConsumerCollection<TConsume> consume, TransformMany<TProduce, TConsume> transform, int threads)
            : this(consume, transform, null, threads)
        {
        }

        public TransformerMany(IProducerConsumerCollection<TConsume> consume, TransformMany<TProduce, TConsume> transform, ITransformer dependentTransformer)
            : this(consume, transform, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        public TransformerMany(IProducerConsumerCollection<TConsume> consume, TransformMany<TProduce, TConsume> transform, ITransformer dependentTransformer, int threads)
            : base(consume, dependentTransformer, threads)
        {
            _transform = transform;
        }

        protected override void ProcessConsume(TConsume consume)
        {
            var results = _transform(consume);
            foreach (var result in results)
            {
                Produce.TryAdd(result);
            }
        }
    }
}
