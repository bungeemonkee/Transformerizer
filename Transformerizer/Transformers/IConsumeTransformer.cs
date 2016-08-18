
using Transformerizer.Collections;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     Defines a <see cref="ITransformer" /> that consumes items of a specific type.
    /// </summary>
    /// <typeparam name="TConsume">The type of items consumed by the transformation.</typeparam>
    public interface IConsumeTransformer<TConsume> : ITransformer
    {
        /// <summary>
        ///     The <see cref="IBlockingQueueRead{T}" /> from which the consumed items are taken.
        /// </summary>
        IBlockingQueueRead<TConsume> Consume { get; }
    }
}