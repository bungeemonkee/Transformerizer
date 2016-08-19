using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transformerizer.Methods;
using Transformerizer.Statistics;
using Transformerizer.Transformers;

namespace Transformerizer
{
    /// <summary>
    ///     Extensions for <see cref="IProduceTransformer{TProduce}" /> objects.
    /// </summary>
    public static class ProduceTransformerExtensions
    {
        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransform<TProduce, TConsume>(this IProduceTransformer<TConsume> transformer, Transform<TProduce, TConsume> transform)
        {
            return new Transformer<TProduce, TConsume>(transformer.Produce, transform, transformer);
        }

        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransform<TProduce, TConsume>(this IProduceTransformer<TConsume> transformer, Transform<TProduce, TConsume> transform, int threads)
        {
            return new Transformer<TProduce, TConsume>(transformer.Produce, transform, transformer, threads);
        }

        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransformMany<TProduce, TConsume>(this IProduceTransformer<TConsume> transformer, TransformMany<TProduce, TConsume> transform)
        {
            return new TransformerMany<TProduce, TConsume>(transformer.Produce, transform, transformer);
        }

        /// <summary>
        ///     Add a new transformation to a chain.
        /// </summary>
        public static IConsumeTransformer<TConsume> ThenTransformVoid<TConsume>(this IProduceTransformer<TConsume> transformer, TransformVoid<TConsume> transform, int threads)
        {
            return new TransformerVoid<TConsume>(transformer.Produce, transform, transformer, threads);
        }

        /// <summary>
        ///     Add a new transformation to end a chain.
        /// </summary>
        public static IConsumeTransformer<TConsume> ThenTransformVoid<TConsume>(this IProduceTransformer<TConsume> transformer, TransformVoid<TConsume> transform)
        {
            return new TransformerVoid<TConsume>(transformer.Produce, transform, transformer);
        }

        /// <summary>
        ///     Add a new transformation to end a chain.
        /// </summary>
        public static ITransformer<TProduce, TConsume> ThenTransformMany<TProduce, TConsume>(this IProduceTransformer<TConsume> transformer, TransformMany<TProduce, TConsume> transform, int threads)
        {
            return new TransformerMany<TProduce, TConsume>(transformer.Produce, transform, transformer, threads);
        }

        /// <summary>
        ///     Ends a transformation by waiting (synchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <returns>The results of the transformation.</returns>
        public static IList<TProduce> EndTransform<TProduce>(this IProduceTransformer<TProduce> transformer)
        {
            return EndTransform(transformer, null);
        }

        /// <summary>
        ///     Ends a transformation by waiting (synchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <param name="statisticsCallback">
        ///     A callback to be invoked with the statistics for this transformer (assuming it
        ///     supports providing statistics).
        /// </param>
        /// <returns>The results of the transformation.</returns>
        public static IList<TProduce> EndTransform<TProduce>(this IProduceTransformer<TProduce> transformer, Action<ITransformerStatistics> statisticsCallback)
        {
            using (transformer)
            {
                transformer.ExecuteAsync().Wait();

                if (statisticsCallback != null)
                {
                    var statistics = (transformer as IStatisticsSource)?.GetStatistics();
                    if (statistics != null)
                    {
                        statisticsCallback(statistics);
                    }
                }

                return transformer.Produce.ToList();
            }
        }

        /// <summary>
        ///     Ends a transformation by waiting (asynchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <returns>The results of the transformation.</returns>
        public static Task<IList<TProduce>> EndTransformAsync<TProduce>(this IProduceTransformer<TProduce> transformer)
        {
            return EndTransformAsync(transformer, null);
        }

        /// <summary>
        ///     Ends a transformation by waiting (asynchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <param name="statisticsCallback">
        ///     A callback to be invoked with the statistics for this transformer (assuming it
        ///     supports providing statistics).
        /// </param>
        /// <returns>The results of the transformation.</returns>
        public static Task<IList<TProduce>> EndTransformAsync<TProduce>(this IProduceTransformer<TProduce> transformer, Action<ITransformerStatistics> statisticsCallback)
        {
            // Create a task completion for when we've gotten the results from the transformer
            var taskCompletionSource = new TaskCompletionSource<IList<TProduce>>();

            try
            {
                // Create the transform task for the actual transformation
                var transformTask = transformer.ExecuteAsync();

                // When the transform is done resolve the task completion
                var arguments = new Tuple<ITransformer, TaskCompletionSource<IList<TProduce>>, Action<ITransformerStatistics>>(transformer, taskCompletionSource, statisticsCallback);
                transformTask.ContinueWith(AsyncUtility.Continuation<TProduce>, arguments);
            }
            catch
            {
                // If there was an exception starting the transformation dispose of the transformer and r-throw the exception
                transformer.Dispose();
                throw;
            }

            // Return the task completion source
            return taskCompletionSource.Task;
        }
    }
}