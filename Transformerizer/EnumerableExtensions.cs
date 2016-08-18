using System.Collections.Generic;
using Transformerizer.Collections;
using Transformerizer.Methods;
using Transformerizer.Transformers;

namespace Transformerizer
{
    /// <summary>
    ///     Extensions for <see cref="IEnumerable{T}" /> to begin transformation chains.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <remarks>
        ///     Manually test several different thread counts for each step in order to optimize your transformation chain.
        ///     Generally IO bound steps should have more threads as they are going to be idle waiting on IO rather than consuming
        ///     CPU.
        ///     Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TProduce">The type of item produced by this transformation.</typeparam>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <param name="threads">The number of threads with which to process the transformation.</param>
        /// <returns>An <see cref="ITransformer{TProduce, TConsume}" /> suitable for parallel processing of transformation steps.</returns>
        public static ITransformer<TProduce, TConsume> BeginTransform<TProduce, TConsume>(this IEnumerable<TConsume> source, Transform<TProduce, TConsume> transform, int threads)
        {
            // Turn the source into an IBlockingQueueRead
            var consume = source as IBlockingQueueRead<TConsume> ?? new BlockingQueueRead<TConsume>(source);

            // Create a thread-safe producer consumer from this enumerable
            return new Transformer<TProduce, TConsume>(consume, transform, threads);
        }

        /// <summary>
        ///     Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <remarks>
        ///     This method will use a default number of threads which is almost certainly sub-optimal.
        ///     Manually test several different thread counts for each step in order to optimize your transformation chain.
        ///     Generally IO bound steps should have more threads as they are going to be idle waiting on IO reather than consuming
        ///     CPU.
        ///     Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TProduce">The type of item produced by this transformation.</typeparam>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <returns>An <see cref="ITransformer{TProduce, TConsume}" /> suitable for parallel processing of transformation steps.</returns>
        public static ITransformer<TProduce, TConsume> BeginTransform<TProduce, TConsume>(this IEnumerable<TConsume> source, Transform<TProduce, TConsume> transform)
        {
            // Turn the source into an IBlockingQueueRead
            var consume = source as IBlockingQueueRead<TConsume> ?? new BlockingQueueRead<TConsume>(source);

            // Create a thread-safe producer consumer from this enumerable
            return new Transformer<TProduce, TConsume>(consume, transform);
        }

        /// <summary>
        ///     Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <remarks>
        ///     Manually test several different thread counts for each step in order to optimize your transformation chain.
        ///     Generally IO bound steps should have more threads as they are going to be idle waiting on IO reather than consuming
        ///     CPU.
        ///     Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TProduce">The type of item produced by this transformation.</typeparam>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <param name="threads">The number of threads with which to process the transformation.</param>
        /// <returns>An <see cref="ITransformer{TProduce, TConsume}" /> suitable for parallel processing of transformation steps.</returns>
        public static ITransformer<TProduce, TConsume> BeginTransformMany<TProduce, TConsume>(this IEnumerable<TConsume> source, TransformMany<TProduce, TConsume> transform, int threads)
        {
            // Turn the source into an IBlockingQueueRead
            var consume = source as IBlockingQueueRead<TConsume> ?? new BlockingQueueRead<TConsume>(source);

            // Create a thread-safe producer consumer from this enumerable
            return new TransformerMany<TProduce, TConsume>(consume, transform, threads);
        }

        /// <summary>
        ///     Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <remarks>
        ///     This method will use a default number of threads which is almost certainly sub-optimal.
        ///     Manually test several different thread counts for each step in order to optimize your transformation chain.
        ///     Generally IO bound steps should have more threads as they are going to be idle waiting on IO rather than consuming
        ///     CPU.
        ///     Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TProduce">The type of item produced by this transformation.</typeparam>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <returns>An <see cref="ITransformer{TProduce, TConsume}" /> suitable for parallel processing of transformation steps.</returns>
        public static ITransformer<TProduce, TConsume> BeginTransformMany<TProduce, TConsume>(this IEnumerable<TConsume> source, TransformMany<TProduce, TConsume> transform)
        {
            // Turn the source into an IBlockingQueueRead
            var consume = source as IBlockingQueueRead<TConsume> ?? new BlockingQueueRead<TConsume>(source);

            // Create a thread-safe producer consumer from this enumerable
            return new TransformerMany<TProduce, TConsume>(consume, transform);
        }

        /// <summary>
        ///     Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <remarks>
        ///     Manually test several different thread counts for each step in order to optimize your transformation chain.
        ///     Generally IO bound steps should have more threads as they are going to be idle waiting on IO reather than consuming
        ///     CPU.
        ///     Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <param name="threads">The number of threads with which to process the transformation.</param>
        /// <returns>An <see cref="IConsumeTransformer{TConsume}" /> suitable for parallel processing of transformation steps.</returns>
        public static IConsumeTransformer<TConsume> BeginTransformVoid<TConsume>(this IEnumerable<TConsume> source, TransformVoid<TConsume> transform, int threads)
        {
            // Turn the source into an IBlockingQueueRead
            var consume = source as IBlockingQueueRead<TConsume> ?? new BlockingQueueRead<TConsume>(source);

            // Create a thread-safe consumer from this enumerable
            return new TransformerVoid<TConsume>(consume, transform, threads);
        }

        /// <summary>
        ///     Begin a parallel transformation chain by consuming any <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <remarks>
        ///     This method will use a default number of threads which is almost certainly sub-optimal.
        ///     Manually test several different thread counts for each step in order to optimize your transformation chain.
        ///     Generally IO bound steps should have more threads as they are going to be idle waiting on IO rather than consuming
        ///     CPU.
        ///     Generally CPU bound steps should have fewer threads as those threads are going to be idle much less.
        /// </remarks>
        /// <typeparam name="TConsume">The type of item consumed by this transformation.</typeparam>
        /// <param name="source">An enumerable of the initial objects.</param>
        /// <param name="transform">The delegate that performs the transformations.</param>
        /// <returns>An <see cref="IConsumeTransformer{TConsume}" /> suitable for parallel processing of transformation steps.</returns>
        public static IConsumeTransformer<TConsume> BeginTransformVoid<TConsume>(this IEnumerable<TConsume> source, TransformVoid<TConsume> transform)
        {
            // Turn the source into an IBlockingQueueRead
            var consume = source as IBlockingQueueRead<TConsume> ?? new BlockingQueueRead<TConsume>(source);

            // Create a thread-safe consumer from this enumerable
            return new TransformerVoid<TConsume>(consume, transform);
        }
    }
}