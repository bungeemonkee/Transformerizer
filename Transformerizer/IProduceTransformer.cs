using System.Collections.Concurrent;

namespace Transformerizer
{
    /// <summary>
    ///     Defines a <see cref="ITransformer" /> that produces items of a specific type.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced by the transformation.</typeparam>
    public interface IProduceTransformer<TProduce> : ITransformer
    {
        /// <summary>
        ///     The <see cref="IProducerConsumerCollection{T}" /> into which the produced items are placed.
        /// </summary>
        IProducerConsumerCollection<TProduce> Produce { get; }
    }
}