using System;

namespace Transformerizer.Statistics
{
    /// <summary>
    ///     See <see cref="IThreadStatistics" />.
    /// </summary>
    public struct ThreadStatistics : IThreadStatistics
    {
        /// <summary>
        ///     See <see cref="IThreadStatistics.TransformCount" />.
        /// </summary>
        public int TransformCount { get; set; }

        /// <summary>
        ///     See <see cref="IThreadStatistics.TimeTotal" />.
        /// </summary>
        public TimeSpan TimeTotal { get; set; }

        /// <summary>
        ///     See <see cref="IThreadStatistics.TimeBufferWait" />.
        /// </summary>
        public TimeSpan TimeBufferWait { get; set; }

        /// <summary>
        ///     See <see cref="IThreadStatistics.TimeToFirstTransform" />.
        /// </summary>
        public TimeSpan TimeToFirstTransform { get; set; }

        /// <summary>
        ///     See <see cref="IThreadStatistics.TimeTransforming" />.
        /// </summary>
        public TimeSpan TimeTransforming { get; set; }
    }
}