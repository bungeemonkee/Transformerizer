using System.Collections.Concurrent;

namespace Transformerizer
{
    /// <summary>
    ///     Defines a <see cref="ITransformer" /> that consumes items of a specific type.
    /// </summary>
    /// <typeparam name="TConsume">The type of items consumed by the transformation.</typeparam>
    public interface IConsumeTransformer<TConsume> : ITransformer
    {
        /// <summary>
        ///     The <see cref="IProducerConsumerCollection{T}" /> from which the consumed items are taken.
        /// </summary>
        IProducerConsumerCollection<TConsume> Consume { get; }
    }
}