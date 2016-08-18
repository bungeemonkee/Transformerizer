namespace Transformerizer.Collections
{
    /// <summary>
    ///     Defines a blocking queue with a count.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBlockingQueueReadCount<T> : IBlockingQueueRead<T>
    {
        /// <summary>
        ///     The count of objects in the queue.
        /// </summary>
        int Count { get; }
    }
}