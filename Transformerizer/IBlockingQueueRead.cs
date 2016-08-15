
using System.Collections.Generic;

namespace Transformerizer
{
    /// <summary>
    /// Defines a blocking queue's read functionality.
    /// </summary>
    public interface IBlockingQueueRead<T> : IEnumerable<T>
    {
        /// <summary>
        /// Whether or not this queue can count its items.
        /// </summary>
        bool HasCount { get; }

        /// <summary>
        /// The count of objects in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Removes one or more items from the queue and returns them in an array.
        /// Blocks until items are removed or there are no more items to remove.
        /// </summary>
        /// <param name="count">The maximum number of items to attempt to take.</param>
        /// <param name="items">The items that were removed from the queue. May be fewer than were requested.</param>
        /// <returns>True if any items were removed. False otherwise.</returns>
        bool TryTake(int count, out T[] items);
    }
}
