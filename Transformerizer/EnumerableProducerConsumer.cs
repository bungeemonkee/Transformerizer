using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Transformerizer
{
    /// <summary>
    /// Wraps an <see cref="IEnumerable{T}"/> with a simple, forward-only, thread-safe <see cref="IProducerConsumerCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class EnumerableProducerConsumer<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private readonly IEnumerator<T> _sourceEnumerator;

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public EnumerableProducerConsumer(IEnumerable<T> source)
        {
            _sourceEnumerator = source.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public T[] ToArray()
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(T item)
        {
            throw new NotImplementedException();
        }

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _sourceEnumerator.Dispose();
        }
    }
}
