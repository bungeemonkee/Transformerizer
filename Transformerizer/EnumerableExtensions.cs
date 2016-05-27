using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Transformerizer
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// Manually test several different thread counts for each step in order to optimize your transformation chain.
        /// Generally IO bound steps should have more threads as they are going to be idle waiting on IO reather than consuming CPU.
        /// Generally CPU bound steps should have fewer threads as they are going to 
        /// </remarks>
        /// <typeparam name="TProduce">The type of item produced by this transformation.</typeparam>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <param name="threads">The number of threads with which to process the transformation.</param>
        /// <returns>An <see cref="ITransformer{TProduce, TConsume}"/> suitable for parallel processing of transformation steps.</returns>
        public static ITransformer<TProduce, TConsume> BeginTransform<TProduce, TConsume>(this IEnumerable<TConsume> source, Transform<TProduce, TConsume> transform, int threads)
        {
            // Turn the source into an IProcuderConsumerCollection
            var consume = CastOrWrap(source);

            // Create a thread-safe producer consumer from this enumerable then use that as the transform source
            return new Transformer<TProduce, TConsume>(consume, transform, threads);
        }

        /// <summary>
        /// Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// This method will use a default number of threads which is almost certainly sub-optimal.
        /// 
        /// Manually test several different thread counts for each step in order to optimize your transformation chain.
        /// Generally IO bound steps should have more threads as they are going to be idle waiting on IO reather than consuming CPU.
        /// Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TProduce">The type of item produced by this transformation.</typeparam>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <returns>An <see cref="ITransformer{TProduce, TConsume}"/> suitable for parallel processing of transformation steps.</returns>
        public static ITransformer<TProduce, TConsume> BeginTransform<TProduce, TConsume>(this IEnumerable<TConsume> source, Transform<TProduce, TConsume> transform)
        {
            // Turn the source into an IProcuderConsumerCollection
            var consume = CastOrWrap(source);

            // Create a thread-safe producer consumer from this enumerable then use that as the transform source
            return new Transformer<TProduce, TConsume>(consume, transform);
        }

        private static IProducerConsumerCollection<T> CastOrWrap<T>(IEnumerable<T> source)
        {
            // If the source already is a producer then use that
            var consume = source as IProducerConsumerCollection<T>;
            if (consume != null) return consume;

            // If the source is already a blocking collection then use that with a wrapper
            var collection = source as BlockingCollection<T>;
            if (collection != null) return new BlockingProducerConsumer<T>(collection);

            // Wrap the enumerable in a thread-safe producer consumer implementation
            return new EnumerableProducerConsumer<T>(source);
        }
    }
}
