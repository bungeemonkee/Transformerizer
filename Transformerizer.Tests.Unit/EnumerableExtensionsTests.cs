using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void BeginTransform_Works_With_General_Enumerable()
        {
            var enumerable = new object[0];
            var result = enumerable.BeginTransform(x => x);
        }

        [TestMethod]
        public void BeginTransform_Works_With_IProducerConsumer()
        {
            var enumerableMock = new Mock<IProducerConsumerCollection<object>>();
            var result = enumerableMock.Object.BeginTransform(x => x);
        }

        [TestMethod]
        public void BeginTransform_Works_With_BlockingCollection()
        {
            var enumerable = new BlockingCollection<object>();
            var result = enumerable.BeginTransform(x => x);
        }

        [TestMethod]
        public void BeginTransformMany_Works_With_General_Enumerable()
        {
            var enumerable = new object[0];
            var result = enumerable.BeginTransformMany(x => new[] {x});
        }

        [TestMethod]
        public void BeginTransformMany_Works_With_IProducerConsumer()
        {
            var enumerableMock = new Mock<IProducerConsumerCollection<object>>();
            var result = enumerableMock.Object.BeginTransformMany(x => new[] {x});
        }

        [TestMethod]
        public void BeginTransformMany_Works_With_BlockingCollection()
        {
            var enumerable = new BlockingCollection<object>();
            var result = enumerable.BeginTransformMany(x => new[] {x});
        }

        [TestMethod]
        public void BeginTransformMany_With_Threads_Works_With_General_Enumerable()
        {
            var enumerable = new object[0];
            var result = enumerable.BeginTransformMany(x => new[] {x}, 1);
        }

        [TestMethod]
        public void BeginTransformMany_With_Threads_Works_With_IProducerConsumer()
        {
            var enumerableMock = new Mock<IProducerConsumerCollection<object>>();
            var result = enumerableMock.Object.BeginTransformMany(x => new[] {x}, 1);
        }

        [TestMethod]
        public void BeginTransformMany_With_Threads_Works_With_BlockingCollection()
        {
            var enumerable = new BlockingCollection<object>();
            var result = enumerable.BeginTransformMany(x => new[] {x}, 1);
        }
    }
}