namespace Transformerizer.Methods
{
    /// <summary>
    ///     A transformation that takes a single item and returns nothing (void).
    /// </summary>
    /// <typeparam name="TConsume">The type of items consumed by this transformation.</typeparam>
    public delegate void TransformVoid<in TConsume>(TConsume consume);
}