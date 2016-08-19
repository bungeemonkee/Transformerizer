using System;

namespace Transformerizer.Statistics
{
    /// <summary>
    /// Statistics for a single trasnformer aggregated across all its threads.
    /// </summary>
    public interface ITransformerStatistics
    {
        /// <summary>
        /// Statistics for any dependent transformer. Ie. the transformer before this one in the chain.
        /// </summary>
        ITransformerStatistics DependentTransformerStatistics { get; }

        /// <summary>
        /// The total number of threads in this transformer.
        /// </summary>
        int ThreadCount { get; }

        /// <summary>
        /// The average time (across all threads) that a single execution of the transformation step took.
        /// </summary>
        TimeSpan IndividualTransformTimeAvg { get; }

        /// <summary>
        /// The set of thread-specific statistics for this transformer.
        /// </summary>
        IThreadStatistics[] ThreadStatisticses { get; }

        /// <summary>
        /// The average time each thread spend in total waiting to get items from the buffer.
        /// </summary>
        TimeSpan TimeBufferWaitAvg { get; }

        /// <summary>
        /// The highest time any one thread spedn waiting for items from the buffer in total.
        /// </summary>
        TimeSpan TimeBufferWaitMax { get; }

        /// <summary>
        /// The lowest time any one thread spent waiting for items form the buffer in total.
        /// </summary>
        TimeSpan TimeBufferWaitMin { get; }

        /// <summary>
        /// The total amount of time actross all threads spent waiting for items from the buffer.
        /// </summary>
        TimeSpan TimeBufferWaitTotal { get; }

        /// <summary>
        /// The average time each thread spent before executing the transform function the first time.
        /// </summary>
        TimeSpan TimeToFirstTransformAvg { get; }

        /// <summary>
        /// The longest time any one thread spent before executing the transform function for the first time.
        /// </summary>
        TimeSpan TimeToFirstTransformMax { get; }

        /// <summary>
        /// The minimum amount of time any one thread spent before executing the transform function for the first time.
        /// </summary>
        TimeSpan TimeToFirstTransformMin { get; }

        /// <summary>
        /// The total amount of time across all threads before each thread executed the transform function for the first time.
        /// </summary>
        TimeSpan TimeToFirstTransformTotal { get; }

        /// <summary>
        /// The average time each thread was alive in total.
        /// </summary>
        TimeSpan TimeTotalAvg { get; }

        /// <summary>
        /// The largest total time any one thread was alive.
        /// </summary>
        TimeSpan TimeTotalMax { get; }

        /// <summary>
        /// The lowest total time any one thread was alive.
        /// </summary>
        TimeSpan TimeTotalMin { get; }

        /// <summary>
        /// The total time all threads were alive.
        /// </summary>
        TimeSpan TimeTotalTotal { get; }

        /// <summary>
        /// The average total time each thread spent running the transformation function.
        /// </summary>
        TimeSpan TimeTransformingAvg { get; }

        /// <summary>
        /// The largest time any single thread spent running the transformation function in total.
        /// </summary>
        TimeSpan TimeTransformingMax { get; }

        /// <summary>
        /// The minimum time any single thread spent running the transformation function in total.
        /// </summary>
        TimeSpan TimeTransformingMin { get; }

        /// <summary>
        /// The total time spent running the transformation function across all threads.
        /// </summary>
        TimeSpan TimeTransformingTotal { get; }

        /// <summary>
        /// The average number of transformations performed on each thread. 
        /// </summary>
        double TransformCountAvg { get; }

        /// <summary>
        /// The most transformations performed by any single thread.
        /// </summary>
        int TransformCountMax { get; }

        /// <summary>
        /// The fewest transformations performed by any single thread.
        /// </summary>
        int TransformCountMin { get; }

        /// <summary>
        /// The total number of transformations performed across all threads.
        /// </summary>
        int TransformCountTotal { get; }

        /// <summary>
        /// The average time across all threads spent waiting for items form the buffer after any threads started the transform function for the first time.
        /// This is a key statistic.
        /// High values likely mean there are more threads than necessary processing a given step.
        ///  </summary>
        TimeSpan TimeIdleAfterFirstTransformAvg { get; }
    }
}