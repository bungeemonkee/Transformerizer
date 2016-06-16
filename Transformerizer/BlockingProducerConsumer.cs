using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Transformerizer
{
    /// <summary>
    /// A simple <see cref="IProducerConsumerCollection{T}"/> that wraps a <see cref="BlockingCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class BlockingProducerConsumer<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private readonly BlockingCollection<T> _collection;

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Create a new BLockingProducerConsumer with an empty <see cref="BlockingCollection{T}"/>.
        /// </summary>
        public BlockingProducerConsumer()
        {
            _collection = new BlockingCollection<T>();
        }

        /// <summary>
        /// Create a new BLockingProducerConsumer with the given <see cref="BlockingCollection{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="BlockingCollection{T}"/> to use as a backing store.</param>
        public BlockingProducerConsumer(BlockingCollection<T> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void CopyTo(T[] array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See <see cref="IProducerConsumerCollection{T}.ToArray()"/>.
        /// </summary>
        public T[] ToArray()
        {
            return _collection.ToArray();
        }

        /// <summary>
        /// See <see cref="IProducerConsumerCollection{T}.TryAdd(T)"/>.
        /// </summary>
        public bool TryAdd(T item)
        {
            return _collection.TryAdd(item, -1);
        }

        /// <summary>
        /// See <see cref="IProducerConsumerCollection{T}.TryTake(out T)"/>.
        /// </summary>
        public bool TryTake(out T item)
        {
            return _collection.TryTake(out item, -1);
        }

        /// <summary>
        /// Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See <see cref="BlockingCollection{T}.CompleteAdding()"/>.
        /// </summary>
        public void CompleteAdding()
        {
            _collection.CompleteAdding();
        }

        /// <summary>
        /// See <see cref="IDisposable.Dispose()"/>.
        /// </summary>
        public void Dispose()
        {
            _collection.Dispose();
        }
    }
}
