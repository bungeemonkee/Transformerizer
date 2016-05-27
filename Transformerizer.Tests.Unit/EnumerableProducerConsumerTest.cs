using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnumerableProducerConsumerTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Count_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var result = source.Count;
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IsSynchronized_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var result = source.IsSynchronized;
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SyncRoot_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var result = source.SyncRoot;
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CopyTo_Array_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var array = (Array)new object[0];
            source.CopyTo(array, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CopyTo_Typed_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var array = new object[0];
            source.CopyTo(array, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetEnumerator_Typed_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var result = source.GetEnumerator();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetEnumerator_UnTyped_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var result = ((IEnumerable)source).GetEnumerator();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ToArray_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            var result = source.ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TryAdd_Throws_NotImplementedException()
        {
            var enumerableMock = new Mock<IEnumerable<object>>();
            var source = new EnumerableProducerConsumer<object>(enumerableMock.Object);
            source.TryAdd(null);
        }
    }
}
