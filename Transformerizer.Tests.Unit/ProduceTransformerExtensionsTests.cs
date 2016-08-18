using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Transformerizer.Collections;
using Transformerizer.Transformers;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProduceTransformerExtensionsTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void EndTransformAsync_Does_Not_Dispose_Transformer_When_ExecuteAsync_Does_Not_Throw_An_Exception()
        {
            var task = new Task(() => { });

            var transformMock = new Mock<ITransformer<int, int>>();
            transformMock
                .Setup(x => x.ExecuteAsync())
                .Returns(task);

            var result = transformMock.Object.EndTransformAsync();

            Assert.IsNotNull(result);
            transformMock.VerifyAll();
            transformMock.Verify(x => x.Dispose(), Times.Never);
        }

        [TestMethod]
        [Timeout(1000)]
        [ExpectedException(typeof(AggregateException))]
        public void EndTransformAsync_ReThrows_Exceptions_From_Continuation()
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            var exception = new Exception();

            var transformMock = new Mock<ITransformer<int, int>>();
            transformMock
                .Setup(x => x.ExecuteAsync())
                .Returns(taskCompletionSource.Task);
            transformMock
                .Setup(x => x.Produce)
                .Throws(exception);

            var result = transformMock.Object.EndTransformAsync();

            taskCompletionSource.SetResult(null);

            result.Wait();
        }

        [TestMethod]
        [Timeout(1000)]
        public void EndTransformAsync_Disposes_Transformer_When_ExecuteAsync_Result_Is_Awaited()
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            var transformMock = new Mock<ITransformer<int, int>>();
            transformMock
                .Setup(x => x.ExecuteAsync())
                .Returns(taskCompletionSource.Task);
            transformMock
                .SetupGet(x => x.Produce)
                .Returns(new BlockingQueue<int>());
            transformMock
                .Setup(x => x.Dispose());

            var result = transformMock.Object.EndTransformAsync();

            taskCompletionSource.SetResult(null);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            transformMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void EndTransformAsync_Disposes_Transformer_When_ExecuteAsync_Throws_Exception()
        {
            var exception = new Exception();

            var transformMock = new Mock<ITransformer<int, int>>();
            transformMock
                .Setup(x => x.ExecuteAsync())
                .Throws(exception);
            transformMock
                .Setup(x => x.Dispose());

            Exception result = null;
            try
            {
                transformMock.Object.EndTransformAsync();
            }
            catch (Exception e)
            {
                result = e;
            }

            Assert.IsNotNull(result);
            transformMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void EndTransformAsync_Cancels_Returned_Task_If_Inner_Task_Is_Canceled()
        {
            var taskSource = new TaskCompletionSource<object>();
            taskSource.SetCanceled();

            var transformMock = new Mock<ITransformer<int, int>>();
            transformMock
                .Setup(x => x.ExecuteAsync())
                .Returns(taskSource.Task);
            transformMock
                .Setup(x => x.Dispose());

            var result = transformMock.Object.EndTransformAsync();

            try
            {
                result.Wait();
            }
            catch (AggregateException exceptions)
            {
                exceptions.Handle(x => x is TaskCanceledException);
            }

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCanceled);
            transformMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void EndTransformAsync_Passes_Exception_From_Inner_Task()
        {
            var exception = new Exception();

            var taskSource = new TaskCompletionSource<object>();
            taskSource.SetException(exception);

            var transformMock = new Mock<ITransformer<int, int>>();
            transformMock
                .Setup(x => x.ExecuteAsync())
                .Returns(taskSource.Task);
            transformMock
                .Setup(x => x.Dispose());

            var result = transformMock.Object.EndTransformAsync();

            try
            {
                result.Wait();
            }
            catch (AggregateException)
            {
            }

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsFaulted);
            Assert.AreSame(exception, result.Exception.InnerException.InnerException);
            transformMock.VerifyAll();
        }
    }
}