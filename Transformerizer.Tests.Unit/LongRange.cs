using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Transformerizer.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class LongRange : IEnumerable<long>
    {
        public long Start { get; }

        public long End { get; }

        public LongRange(long start, long end)
        {
            Start = start;
            End = end;
        }

        public IEnumerator<long> GetEnumerator()
        {
            var current = Start;
            while (current <= End)
            {
                yield return current++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
