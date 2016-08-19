using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transformerizer.Statistics;
using Transformerizer.Transformers;

namespace Transformerizer
{
    internal static class AsyncUtility
    {
        public static void Continuation<T>(Task task, object state)
        {
            var arguments = (Tuple<ITransformer, TaskCompletionSource<IList<T>>, Action<ITransformerStatistics>>)state;

            try
            {
                switch (task.Status)
                {
                    case TaskStatus.Canceled:
                        // Set the task result to canceled
                        arguments.Item2.TrySetCanceled();
                        break;
                    case TaskStatus.Faulted:
                        // Set the task result to the exception in the transformation task
                        arguments.Item2.TrySetException(task.Exception);
                        break;
                    case TaskStatus.RanToCompletion:
                        // If there is a statistics callback then process the statistics
                        if (arguments.Item3 != null)
                        {
                            var statistics = (arguments.Item1 as IStatisticsSource)?.GetStatistics();
                            arguments.Item3(statistics);
                        }

                        // If there are results then return them (or just return null)
                        var results = (arguments.Item1 as IProduceTransformer<T>)?.Produce.ToList();
                        arguments.Item2.TrySetResult(results);
                        break;
                    default:
                        throw new InvalidOperationException($"Continuation from task in status: {task.Status}");
                }
            }
            catch (Exception exception)
            {
                arguments.Item2.TrySetException(exception);
            }
            finally
            {
                // Dispose of the transformer now that we got everything from it
                arguments.Item1.Dispose();
            }
        }
    }
}
