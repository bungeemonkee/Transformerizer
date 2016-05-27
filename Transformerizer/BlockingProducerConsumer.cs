using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Transformerizer
{
    public class BlockingProducerConsumer<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private readonly BlockingCollection<T> _collection;

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

        public BlockingProducerConsumer()
        {
            _collection = new BlockingCollection<T>();
        }

        public BlockingProducerConsumer(BlockingCollection<T> collection)
        {
            _collection = collection;
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
            return _collection.ToArray();
        }

        public bool TryAdd(T item)
        {
            return _collection.TryAdd(item, -1);
        }

        public bool TryTake(out T item)
        {
            return _collection.TryTake(out item, -1);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CompleteAdding()
        {
            _collection.CompleteAdding();
        }

        public void Dispose()
        {
            _collection.Dispose();
        }
    }
}
