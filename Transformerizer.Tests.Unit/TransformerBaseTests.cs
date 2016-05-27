using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TransformerBaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExecuteAsync_Throws_InvalidOperationException_When_Called_Twice()
        {
            var producerConsumerMock = new Mock<IProducerConsumerCollection<object>>();
            var transformerMock = new Mock<TransformerBase<object, object>>(producerConsumerMock.Object);

            object result;
            try
            {
                result = transformerMock.Object.ExecuteAsync();
            }
            catch
            {
                result = null;
            }

            // We have to be sure that the exception is only coming from the second method invocation
            Assert.IsNotNull(result);

            transformerMock.Object.ExecuteAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Throws_ArugmentNullException_When_Consume_Collection_Is_Null()
        {
            var trasnformer = new Transformer<object, object>(null, null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_Throws_ArgumentOutOfRangeException_Thread_Count_Is_Below_One()
        {
            var collectionMock = new Mock<IProducerConsumerCollection<object>>();
            var transformerMock = new Transformer<object, object>(collectionMock.Object, null, 0);
        }
    }
}
