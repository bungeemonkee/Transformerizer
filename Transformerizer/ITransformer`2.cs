using System.Collections.Concurrent;

namespace Transformerizer
{
    /// <summary>
    ///     Defines a <see cref="ITransformer" /> that produces and consumes specific types.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced by the transformation.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed by the transformation.</typeparam>
    public interface ITransformer<TProduce, TConsume> : IProduceTransformer<TProduce>, IConsumeTransformer<TConsume>
    {
    }
}