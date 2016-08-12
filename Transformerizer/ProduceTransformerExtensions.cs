using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transformerizer
{
    /// <summary>
    /// Extensions for <see cref="IProduceTransformer{TProduce}"/> objects.
    /// </summary>
    public static class ProduceTransformerExtensions
    {
        /// <summary>
        ///     Ends a transformation by waiting (synchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <returns>The results of the transformation.</returns>
        public static IList<TProduce> EndTransform<TProduce>(this IProduceTransformer<TProduce> transformer)
        {
            using (transformer)
            {
                transformer.ExecuteAsync().Wait();
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
            // Create a task completion for when we've gotten the results from the transformer
            var taskCompletionSource = new TaskCompletionSource<IList<TProduce>>();

            try
            {
                // Create the transform task for the actual transformation
                var transformTask = transformer.ExecuteAsync();

                // When the transform is done resolve the task completion
                transformTask.ContinueWith(t =>
                {
                    try
                    {
                        switch (t.Status)
                        {
                            case TaskStatus.Canceled:
                                // Set the task result to canceled
                                taskCompletionSource.SetCanceled();
                                break;
                            case TaskStatus.Faulted:
                                // Set the task result to the exception in the transformation task
                                taskCompletionSource.SetException(t.Exception);
                                break;
                            default:
                                // Set the task result to the transformer's production collection
                                taskCompletionSource.SetResult(transformer.Produce.ToList());
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        taskCompletionSource.TrySetException(exception);
                    }
                    finally
                    {
                        // Dispose of the transformer now that we got everything from it
                        transformer.Dispose();
                    }
                });
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
