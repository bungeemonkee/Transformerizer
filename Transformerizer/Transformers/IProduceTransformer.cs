using Transformerizer.Collections;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     Defines a <see cref="ITransformer" /> that produces items of a specific type.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced by the transformation.</typeparam>
    public interface IProduceTransformer<TProduce> : ITransformer
    {
        /// <summary>
        ///     The <see cref="IBlockingQueue{T}" /> into which the produced items are placed.
        /// </summary>
        IBlockingQueue<TProduce> Produce { get; }
    }
}