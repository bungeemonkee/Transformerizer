using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BlockingProducerConsumerTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Count_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.Count;
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IsSyncronized_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.IsSynchronized;
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
        [ExpectedException(typeof(NotImplementedException))]
        public void GetEnumerator_Typed_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = collection.GetEnumerator();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetEnumerator_Untyped_Throws_NotImplemented_Exception()
        {
            var collection = new BlockingProducerConsumer<object>();
            var result = ((IEnumerable) collection).GetEnumerator();
        }
    }
}