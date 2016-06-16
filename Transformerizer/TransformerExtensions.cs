using System;
using System.Threading.Tasks;

namespace Transformerizer
{
    /// <summary>
    ///     Extensions for <see cref="ITransformer{TProduce,TConsume}" /> that continue and end transformation chains.
    /// </summary>
    public static class TransformerExtensions
    {
        /// <summary>
        ///     Ends a transformation by waiting (synchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <returns>The results of the transformation.</returns>
        public static TProduce[] EndTransform<TProduce, TConsume>(this ITransformer<TProduce, TConsume> transformer)
        {
            using (transformer)
            {
                transformer.ExecuteAsync().Wait();
                return transformer.Produce.ToArray();
            }
        }

        /// <summary>
        ///     Ends a transformation by waiting (asynchronously) for it to finish.
        /// </summary>
        /// <typeparam name="TProduce">The type of the items produced.</typeparam>
        /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
        /// <param name="transformer">The transformer to finish.</param>
        /// <returns>The results of the transformation.</returns>
        public static Task<TProduce[]> EndTransformAsync<TProduce, TConsume>(this ITransformer<TProduce, TConsume> transformer)
        {
            // Create a task completion for when we've gotten the results from the transformer
            var taskCompletionSource = new TaskCompletionSource<TProduce[]>();

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
                                // Set the task result to transformer's production collection
                                taskCompletionSource.SetResult(transformer.Produce.ToArray());
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