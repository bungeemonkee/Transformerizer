using System.Collections.Generic;

namespace Transformerizer.Collections
{
    /// <summary>
    ///     Defines a blocking queue's functionality.
    /// </summary>
    public interface IBlockingQueue<T> : IBlockingQueueReadCount<T>
    {
        /// <summary>
        ///     Marks the queue so that any future insertions will fail.
        /// </summary>
        void CompleteAdding();

        /// <summary>
        ///     Adds several objects at once to the queue.
        ///     Blocks until the items are added.
        /// </summary>
        /// <param name="items">The items to be added to the queue.</param>
        /// <returns>True if the items are added. False if <see cref="CompleteAdding()" /> has alreay been called.</returns>
        bool TryAdd(IEnumerable<T> items);

        /// <summary>
        ///     Adds a single item to the queue.
        /// </summary>
        /// <param name="item">The item to be added to the queue.</param>
        /// <returns>True if the items are added. False if <see cref="CompleteAdding()" /> has alreay been called.</returns>
        bool TryAdd(T item);
    }
}