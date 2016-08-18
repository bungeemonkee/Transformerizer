using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Transformerizer.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class Fibonacci : IEnumerable<long>
    {
        public long Max { get; }

        public Fibonacci(long max)
        {
            Max = max;
        }

        public IEnumerator<long> GetEnumerator()
        {
            var last = 1L;
            var current = 1L;
            while (current <= Max)
            {
                yield return current;

                var next = current + last;
                last = current;
                current = next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}