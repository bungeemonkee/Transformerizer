using System.Collections.Concurrent;

namespace Transformerizer
{
    public class Transformer<TProduce, TConsume> : TransformerBase<TProduce, TConsume>
    {

        private readonly Transform<TProduce, TConsume> _transform;

        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform)
            : this(consume, transform, null, DefaultThreadCount)
        {
        }

        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform, int threads)
            : this(consume, transform, null, threads)
        {
        }

        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform, ITransformer dependentTransformer)
            : this(consume, transform, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        public Transformer(IProducerConsumerCollection<TConsume> consume, Transform<TProduce, TConsume> transform, ITransformer dependentTransformer, int threads)
            : base(consume, dependentTransformer, threads)
        {
            _transform = transform;
        }

        protected override void ProcessConsume(TConsume consume)
        {
            var result = _transform(consume);
            Produce.TryAdd(result);
        }
    }
}
