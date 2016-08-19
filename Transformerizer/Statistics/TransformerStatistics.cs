using System;
using System.Collections.Generic;
using System.Linq;

namespace Transformerizer.Statistics
{
    /// <summary>
    ///     See <see cref="ITransformerStatistics" />.
    /// </summary>
    public struct TransformerStatistics : ITransformerStatistics
    {
        /// <summary>
        ///     See <see cref="ITransformerStatistics.DependentTransformerStatistics" />.
        /// </summary>
        public ITransformerStatistics DependentTransformerStatistics { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.ThreadCount" />.
        /// </summary>
        public int ThreadCount { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.ThreadStatisticses" />.
        /// </summary>
        public IThreadStatistics[] ThreadStatisticses { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TransformCountTotal" />.
        /// </summary>
        public int TransformCountTotal { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TransformCountMin" />.
        /// </summary>
        public int TransformCountMin { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TransformCountMax" />.
        /// </summary>
        public int TransformCountMax { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TransformCountAvg" />.
        /// </summary>
        public double TransformCountAvg { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTotalTotal" />.
        /// </summary>
        public TimeSpan TimeTotalTotal { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTotalMin" />.
        /// </summary>
        public TimeSpan TimeTotalMin { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTotalMax" />.
        /// </summary>
        public TimeSpan TimeTotalMax { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTotalAvg" />.
        /// </summary>
        public TimeSpan TimeTotalAvg { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeBufferWaitTotal" />.
        /// </summary>
        public TimeSpan TimeBufferWaitTotal { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeBufferWaitMin" />.
        /// </summary>
        public TimeSpan TimeBufferWaitMin { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeBufferWaitMax" />.
        /// </summary>
        public TimeSpan TimeBufferWaitMax { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeBufferWaitAvg" />.
        /// </summary>
        public TimeSpan TimeBufferWaitAvg { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeToFirstTransformTotal" />.
        /// </summary>
        public TimeSpan TimeToFirstTransformTotal { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeToFirstTransformMin" />.
        /// </summary>
        public TimeSpan TimeToFirstTransformMin { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeToFirstTransformMax" />.
        /// </summary>
        public TimeSpan TimeToFirstTransformMax { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeToFirstTransformAvg" />.
        /// </summary>
        public TimeSpan TimeToFirstTransformAvg { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTransformingTotal" />.
        /// </summary>
        public TimeSpan TimeTransformingTotal { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTransformingMin" />.
        /// </summary>
        public TimeSpan TimeTransformingMin { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTransformingMax" />.
        /// </summary>
        public TimeSpan TimeTransformingMax { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.TimeTransformingAvg" />.
        /// </summary>
        public TimeSpan TimeTransformingAvg { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.IndividualTransformTimeAvg" />.
        /// </summary>
        public TimeSpan IndividualTransformTimeAvg { get; }

        /// <summary>
        ///     See <see cref="ITransformerStatistics.IndividualTransformTimeAvg" />.
        /// </summary>
        public TimeSpan TimeIdleAfterFirstTransformAvg { get; }

        /// <summary>
        ///     Create a new TransformerStatistics from a collection of thread
        /// </summary>
        public TransformerStatistics(IEnumerable<IThreadStatistics> threadStatisticses, ITransformerStatistics dependentTransformerStatistics)
            : this()
        {
            DependentTransformerStatistics = dependentTransformerStatistics;

            ThreadStatisticses = threadStatisticses as IThreadStatistics[] ?? threadStatisticses.ToArray();

            ThreadCount = ThreadStatisticses.Length;

            TransformCountTotal = ThreadStatisticses.Sum(x => x.TransformCount);
            TransformCountMin = ThreadStatisticses.Max(x => x.TransformCount);
            TransformCountMax = ThreadStatisticses.Min(x => x.TransformCount);
            TransformCountAvg = ThreadStatisticses.Average(x => x.TransformCount);

            TimeTotalTotal = TimeSpan.FromTicks(ThreadStatisticses.Sum(x => x.TimeTotal.Ticks));
            TimeTotalMin = TimeSpan.FromTicks(ThreadStatisticses.Min(x => x.TimeTotal.Ticks));
            TimeTotalMax = TimeSpan.FromTicks(ThreadStatisticses.Max(x => x.TimeTotal.Ticks));
            TimeTotalAvg = TimeSpan.FromTicks((long) ThreadStatisticses.Average(x => x.TimeTotal.Ticks));

            TimeBufferWaitTotal = TimeSpan.FromTicks(ThreadStatisticses.Sum(x => x.TimeBufferWait.Ticks));
            TimeBufferWaitMin = TimeSpan.FromTicks(ThreadStatisticses.Min(x => x.TimeBufferWait.Ticks));
            TimeBufferWaitMax = TimeSpan.FromTicks(ThreadStatisticses.Max(x => x.TimeBufferWait.Ticks));
            TimeBufferWaitAvg = TimeSpan.FromTicks((long) ThreadStatisticses.Average(x => x.TimeBufferWait.Ticks));

            TimeToFirstTransformTotal = TimeSpan.FromTicks(ThreadStatisticses.Sum(x => x.TimeToFirstTransform.Ticks));
            TimeToFirstTransformMin = TimeSpan.FromTicks(ThreadStatisticses.Min(x => x.TimeToFirstTransform.Ticks));
            TimeToFirstTransformMax = TimeSpan.FromTicks(ThreadStatisticses.Max(x => x.TimeToFirstTransform.Ticks));
            TimeToFirstTransformAvg = TimeSpan.FromTicks((long) ThreadStatisticses.Average(x => x.TimeToFirstTransform.Ticks));

            TimeTransformingTotal = TimeSpan.FromTicks(ThreadStatisticses.Sum(x => x.TimeTransforming.Ticks));
            TimeTransformingMin = TimeSpan.FromTicks(ThreadStatisticses.Min(x => x.TimeTransforming.Ticks));
            TimeTransformingMax = TimeSpan.FromTicks(ThreadStatisticses.Max(x => x.TimeTransforming.Ticks));
            TimeTransformingAvg = TimeSpan.FromTicks((long) ThreadStatisticses.Average(x => x.TimeTransforming.Ticks));

            IndividualTransformTimeAvg = TimeSpan.FromTicks(TimeTransformingTotal.Ticks/TransformCountTotal);

            var firstTransformTicks = TimeToFirstTransformMin.Ticks;
            TimeIdleAfterFirstTransformAvg = TimeSpan.FromTicks((long) ThreadStatisticses.Average(x => x.TimeBufferWait.Ticks - firstTransformTicks));
        }
    }
}