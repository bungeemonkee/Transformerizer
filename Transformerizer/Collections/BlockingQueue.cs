using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Transformerizer.Collections
{
    /// <summary>
    ///     An <see cref="IBlockingQueue{T}" />.
    /// </summary>
    public class BlockingQueue<T> : IBlockingQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly SemaphoreSlim _modifySemaphore = new SemaphoreSlim(1, 1);
        private readonly ManualResetEvent _addedHandle = new ManualResetEvent(false);
        private volatile bool _completeAdding;
        
        /// <summary>
        ///     See <see cref="IBlockingQueueReadCount{T}.Count" />.
        /// </summary>
        public int Count => _queue.Count;

        /// <summary>
        ///     See <see cref="IBlockingQueue{T}.CompleteAdding()" />.
        /// </summary>
        public void CompleteAdding()
        {
            _completeAdding = true;

            // Setting the handle needs to be syncronized with any queue modifications
            _modifySemaphore.Wait();

            // Unblock any waiting attempts to take from the queue so they can fail
            _addedHandle.Set();

            // Allow any atempts to take from the queue to proceed
            _modifySemaphore.Release();
        }

        /// <summary>
        ///     See <see cref="IBlockingQueue{T}.TryAdd(T)" />.
        /// </summary>
        public bool TryAdd(T item)
        {
            return TryAdd(Enumerable.Repeat(item, 1));
        }

        /// <summary>
        ///     See <see cref="IBlockingQueue{T}.TryAdd(IEnumerable{T})" />.
        /// </summary>
        public bool TryAdd(IEnumerable<T> items)
        {
            // If adding isn't allowed anymore then just fail
            if (_completeAdding) return false;

            // Wait for the modification semaphore
            _modifySemaphore.Wait();

            // Put all the items in the queue
            foreach (var item in items)
            {
                _queue.Enqueue(item);
            }

            // Signal that an add has occured
            _addedHandle.Set();

            // Release the modification semaphore
            _modifySemaphore.Release();

            // Add complete
            return true;
        }

        /// <summary>
        ///     See <see cref="IBlockingQueueRead{T}.TryTake(int,out T[])" />.
        /// </summary>
        public bool TryTake(int count, out T[] items)
        {
            // Wait for the modification semaphore
            _modifySemaphore.Wait();

            // Verify that there is anything to take
            while (_queue.Count <= 0)
            {
                // If adding is complete then fail
                if (_completeAdding)
                {
                    // Unblock any other take or add operations
                    _modifySemaphore.Release();

                    // Failure
                    items = null;
                    return false;
                }

                // Reset the add handle to allow this thread to wait for an add
                _addedHandle.Reset();

                // Unblock any other take or add operations
                _modifySemaphore.Release();

                // Wait for an add to occur
                _addedHandle.WaitOne();

                // Now there MAY be things added so get the modification lock again
                _modifySemaphore.Wait();
            }

            // Determine how many items to actually take from the queue
            count = Math.Min(count, _queue.Count);

            // Create the result array
            items = new T[count];

            // Collect items from the queue into the result array
            for (var i = 0; i < count; ++i)
            {
                items[i] = _queue.Dequeue();
            }

            // Release the modification semaphore
            _modifySemaphore.Release();

            // Success
            return true;
        }

        /// <summary>
        ///     See <see cref="IEnumerable{T}.GetEnumerator()" />.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
