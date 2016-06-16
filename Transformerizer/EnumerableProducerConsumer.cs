using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Transformerizer
{
    /// <summary>
    ///     Wraps an <see cref="IEnumerable{T}" /> with a simple, forward-only, thread-safe
    ///     <see cref="IProducerConsumerCollection{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class EnumerableProducerConsumer<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private readonly IEnumerator<T> _sourceEnumerator;

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Creates a new EnumerableProducerConsumer using the given <see cref="IEnumerable{T}" /> as a data source.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}" /> to use as a source for all items.</param>
        public EnumerableProducerConsumer(IEnumerable<T> source)
        {
            _sourceEnumerator = source.GetEnumerator();
        }

        /// <summary>
        ///     See <see cref="IDisposable.Dispose()" />.
        /// </summary>
        public void Dispose()
        {
            _sourceEnumerator.Dispose();
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void CopyTo(T[] array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public T[] ToArray()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public bool TryAdd(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     See <see cref="IProducerConsumerCollection{T}.TryTake(out T)" />.
        /// </summary>
        public bool TryTake(out T item)
        {
            lock (_sourceEnumerator)
            {
                if (_sourceEnumerator.MoveNext())
                {
                    // There is a next item - set the item and return true
                    item = _sourceEnumerator.Current;
                    return true;
                }
            }

            // There is no next value - return a default and false
            item = default(T);
            return false;
        }

        /// <summary>
        ///     Always throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}