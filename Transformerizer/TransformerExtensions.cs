using System.Threading.Tasks;

namespace Transformerizer
{
    public static class TransformerExtensions
    {
        public static TProduce[] EndTransform<TProduce, TConsume>(this ITransformer<TProduce, TConsume> transformer)
        {
            using (transformer)
            {
                transformer.ExecuteAsync().Wait();
                return transformer.Produce.ToArray();
            }
        }

        public static Task<TProduce[]> EndTransformAsync<TProduce, TConsume>(this ITransformer<TProduce, TConsume> transformer)
        {
            // Create a task completion for when we've gotten the results from the transformer
            var taskCompletionSource = new TaskCompletionSource<TProduce[]>();

            // Create the transform task for the actual transformation
            var transformTask = transformer.ExecuteAsync();

            // When the transform is done resolve the task completion
            transformTask.ContinueWith(t =>
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

                // Dispose of the transformer now that we got everything from it
                transformer.Dispose();
            });

            // Return the task completion source
            return taskCompletionSource.Task;
        }

        public static ITransformer<TProduce, TConsume> ThenTransform<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, Transform<TProduce, TConsume> transform)
        {
            return new Transformer<TProduce, TConsume>(transformer.Produce, transform, transformer);
        }

        public static ITransformer<TProduce, TConsume> ThenTransform<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, Transform<TProduce, TConsume> transform, int threads)
        {
            return new Transformer<TProduce, TConsume>(transformer.Produce, transform, transformer, threads);
        }

        public static ITransformer<TProduce, TConsume> ThenTransformMany<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, TransformMany<TProduce, TConsume> transform)
        {
            return new TransformerMany<TProduce, TConsume>(transformer.Produce, transform, transformer);
        }

        public static ITransformer<TProduce, TConsume> ThenTransformMany<TProduce, TConsume, TOriginal>(this ITransformer<TConsume, TOriginal> transformer, TransformMany<TProduce, TConsume> transform, int threads)
        {
            return new TransformerMany<TProduce, TConsume>(transformer.Produce, transform, transformer, threads);
        }
    }
}
