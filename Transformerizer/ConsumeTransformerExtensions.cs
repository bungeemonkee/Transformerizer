using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transformerizer.Statistics;
using Transformerizer.Transformers;

namespace Transformerizer
{
    /// <summary>
    ///     Extensions for <see cref="IConsumeTransformer{TConsume}" /> objects.
    /// </summary>
    public static class ConsumeTransformerExtensions
    {
        /// <summary>
        ///     Ends a transformation by waiting (synchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TConsume">The type of the items consumed by .</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        public static void EndTransformVoid<TConsume>(this IConsumeTransformer<TConsume> transformer)
        {
            EndTransformVoid(transformer, null);
        }

        /// <summary>
        ///     Ends a transformation by waiting (synchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TConsume">The type of the items consumed by .</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <param name="statisticsCallback">
        ///     A callback to be invoked with the statistics for this transformer (assuming it
        ///     supports providing statistics).
        /// </param>
        public static void EndTransformVoid<TConsume>(this IConsumeTransformer<TConsume> transformer, Action<ITransformerStatistics> statisticsCallback)
        {
            using (transformer)
            {
                transformer.ExecuteAsync().Wait();

                if (statisticsCallback == null) return;

                var statistics = (transformer as IStatisticsSource)?.GetStatistics();
                if (statistics != null)
                {
                    statisticsCallback(statistics);
                }
            }
        }

        /// <summary>
        ///     Ends a transformation by waiting (asynchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <returns>The results of the transformation.</returns>
        public static Task EndTransformVoidAsync<TConsume>(this IConsumeTransformer<TConsume> transformer)
        {
            return EndTransformVoidAsync(transformer, null);
        }

        /// <summary>
        ///     Ends a transformation by waiting (asynchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <param name="statisticsCallback">
        ///     A callback to be invoked with the statistics for this transformer (assuming it
        ///     supports providing statistics).
        /// </param>
        /// <returns>The results of the transformation.</returns>
        public static Task EndTransformVoidAsync<TConsume>(this IConsumeTransformer<TConsume> transformer, Action<ITransformerStatistics> statisticsCallback)
        {
            // Create a task completion for when we've gotten the results from the transformer
            var taskCompletionSource = new TaskCompletionSource<IList<object>>();

            try
            {
                // Create the transform task for the actual transformation
                var transformTask = transformer.ExecuteAsync();

                // When the transform is done resolve the task completion
                var arguments = new Tuple<ITransformer, TaskCompletionSource<IList<object>>, Action<ITransformerStatistics>>(transformer, taskCompletionSource, statisticsCallback);
                transformTask.ContinueWith(AsyncUtility.Continuation<object>, arguments);
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