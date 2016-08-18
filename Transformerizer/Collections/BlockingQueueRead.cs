using System;
using System.Collections;
using System.Collections.Generic;

namespace Transformerizer.Collections
{
    /// <summary>
    ///     An <see cref="IBlockingQueueRead{T}" /> that wraps an <see cref="IEnumerable{T}" /> to provide simple forward-only
    ///     access.
    /// </summary>
    public class BlockingQueueRead<T> : IBlockingQueueRead<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly IEnumerator<T> _enumerator;

        /// <summary>
        ///     Create a new <see cref="BlockingQueueRead{T}" /> that reads from the given source.
        /// </summary>
        public BlockingQueueRead(IEnumerable<T> source)
        {
            _source = source;
            _enumerator = source.GetEnumerator();
        }

        /// <summary>
        ///     See <see cref="IBlockingQueueRead{T}.TryTake(int, out T[])" />.
        /// </summary>
        public bool TryTake(int count, out T[] items)
        {
            var any = false;

            items = new T[count];

            lock (this)
            {
                for (count = 0; count < items.Length && _enumerator.MoveNext(); ++count)
                {
                    any = true;
                    items[count] = _enumerator.Current;
                }
            }

            if (!any)
            {
                items = null;
                return false;
            }

            if (count != items.Length)
            {
                // If there weren't enough items in the source then re-size the array to be smaller
                Array.Resize(ref items, count);
            }

            return true;
        }

        /// <summary>
        ///     See <see cref="IEnumerable{T}.GetEnumerator()" />.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}