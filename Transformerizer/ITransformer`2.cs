
using System.Collections.Concurrent;

namespace Transformerizer
{
    /// <summary>
    /// Defines a <see cref="ITransformer"/> that produces and consumers specific types.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced by the transformation.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed by the transformation.</typeparam>
    public interface ITransformer<TProduce, TConsume> : ITransformer
    {
        /// <summary>
        /// The <see cref="IProducerConsumerCollection{T}"/> into which the produced items are placed.
        /// </summary>
        IProducerConsumerCollection<TProduce> Produce { get; }

        /// <summary>
        /// The <see cref="IProducerConsumerCollection{T}"/> from which the consumed items are taken.
        /// </summary>
        IProducerConsumerCollection<TConsume> Consume { get; }
    }
}
