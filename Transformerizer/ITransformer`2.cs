
using System.Collections.Concurrent;

namespace Transformerizer
{
    public interface ITransformer<TProduce, TConsume> : ITransformer
    {
        IProducerConsumerCollection<TProduce> Produce { get; }

        IProducerConsumerCollection<TConsume> Consume { get; }
    }
}
