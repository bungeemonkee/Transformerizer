using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BlockingProducerConsumerTests
    {
        [TestMethod]
        public void Count_Returns_BlockingCollection_Count()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void IsSyncronized_Returns_True()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.IsSynchronized;
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SyncRoot_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.SyncRoot;
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CopyTo_Typed_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var array = new object[0];
            collection.CopyTo(array, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CopyTo_Array_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var array = (Array) new object[0];
            collection.CopyTo(array, 0);
        }

        [TestMethod]
        public void GetEnumerator_Typed_Returns_Enumerator()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.GetEnumerator();
            Assert.IsInstanceOfType(result, typeof(IEnumerator<object>));
        }

        [TestMethod]
        public void GetEnumerator_Untyped_Returns_Enumerator()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = ((IEnumerable) collection).GetEnumerator();
            Assert.IsInstanceOfType(result, typeof(IEnumerator<object>));
        }
    }
}