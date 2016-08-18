using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transformerizer.Collections;

namespace Transformerizer.Tests.Unit.Collections
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BlockingQueueTests
    {
        [TestMethod]
        public void TryAdd_Fails_If_CompleteAdding_Has_Been_Called()
        {
            var queue = new BlockingQueue<int>();

            queue.CompleteAdding();

            var result = queue.TryAdd(1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IEnumerable_GetEnumerator_Works()
        {
            var queue = new BlockingQueue<int>();

            var result = ((IEnumerable) queue).GetEnumerator();

            Assert.IsNotNull(result);
        }
    }
}