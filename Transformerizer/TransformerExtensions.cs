
namespace Transformerizer
{
    /// <summary>
    ///     Extensions for <see cref="ITransformer{TProduce,TConsume}" /> that continue and end transformation chains.
    /// </summary>
    public static class TransformerExtensions
    {
        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransform<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, Transform<TProduce, TConsume> transform)
        {
            return new Transformer<TProduce, TConsume>(transformer.Produce, transform, transformer);
        }

        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransform<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, Transform<TProduce, TConsume> transform, int threads)
        {
            return new Transformer<TProduce, TConsume>(transformer.Produce, transform, transformer, threads);
        }

        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransformMany<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, TransformMany<TProduce, TConsume> transform)
        {
            return new TransformerMany<TProduce, TConsume>(transformer.Produce, transform, transformer);
        }

        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransformMany<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, TransformMany<TProduce, TConsume> transform, int threads)
        {
            return new TransformerMany<TProduce, TConsume>(transformer.Produce, transform, transformer, threads);
        }
    }
}