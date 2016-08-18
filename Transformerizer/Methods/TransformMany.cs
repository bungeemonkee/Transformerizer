using System.Collections.Generic;

namespace Transformerizer.Methods
{
    /// <summary>
    ///     A transformation that takes a single item and returns a collection of zero or more items.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced by this transformation.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed by this transformation.</typeparam>
    /// <param name="consume">The item to transform.</param>
    /// <returns>The collection of transformed items.</returns>
    public delegate IEnumerable<TProduce> TransformMany<out TProduce, in TConsume>(TConsume consume);
}