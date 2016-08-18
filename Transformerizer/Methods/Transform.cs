namespace Transformerizer.Methods
{
    /// <summary>
    ///     A transformation that takes a single item and returns a single item.
    /// </summary>
    /// <typeparam name="TProduce">The type of items produced by this transformation.</typeparam>
    /// <typeparam name="TConsume">The type of items consumed by this transformation.</typeparam>
    /// <param name="consume">The item to transform.</param>
    /// <returns>The transformed item.</returns>
    public delegate TProduce Transform<out TProduce, in TConsume>(TConsume consume);
}