using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transformerizer.Collections;

namespace Transformerizer.Tests.Unit.Collections
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BlockingQueueReadTests
    {
        [TestMethod]
        public void GetEnumerator_Works()
        {
            var queue = new BlockingQueueRead<int>(Enumerable.Empty<int>());

            var result = queue.GetEnumerator();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IEnumerable_GetEnumerator_Works()
        {
            var queue = new BlockingQueueRead<int>(Enumerable.Empty<int>());

            var result = ((IEnumerable) queue).GetEnumerator();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TryTake_Returns_Fewer_Items_If_Enough_Are_Not_Available()
        {
            var source = new[] {1, 2, 3, 4, 5};
            var queue = new BlockingQueueRead<int>(source);

            int[] value;
            var result = queue.TryTake(source.Length*2, out value);

            Assert.IsTrue(result);
            Assert.IsNotNull(value);
            Assert.AreEqual(source.Length, value.Length);
            CollectionAssert.AreEqual(source, value);
        }
    }
}