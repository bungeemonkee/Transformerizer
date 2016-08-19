using System;

namespace Transformerizer.Statistics
{
    /// <summary>
    ///     Statistics about a single thread in a trasnformation.
    /// </summary>
    public interface IThreadStatistics
    {
        /// <summary>
        ///     The number of times this thread invoked the transformation function.
        /// </summary>
        int TransformCount { get; }

        /// <summary>
        ///     The total time this thread spent in the process method.
        /// </summary>
        TimeSpan TimeTotal { get; }

        /// <summary>
        ///     The total time this thread spent waiting to get items out of the buffer.
        /// </summary>
        TimeSpan TimeBufferWait { get; }

        /// <summary>
        ///     The total time until this thread executes its transformation the first time.
        /// </summary>
        TimeSpan TimeToFirstTransform { get; }

        /// <summary>
        ///     The total time this thread spent in the transformation method.
        /// </summary>
        TimeSpan TimeTransforming { get; }
    }
}