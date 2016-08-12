using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Transformerizer.Tests.Unit
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
    }
}
