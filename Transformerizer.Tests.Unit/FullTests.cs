using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FullTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void FullTest_01()
        {
            var strings = new[] { "1", "2", "3", "4", "5" };
            var numbers = new[] { 10, 20, 30, 40, 50 };

            var result = strings
                .BeginTransform(int.Parse, 1)
                .ThenTransform(x => x * 10)
                .EndTransform()
                .ToArray();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        [Timeout(1000)]
        public void FullTest_02()
        {
            var strings = new[] { "1", "2", "3", "4", "5" };
            var numbers = new[] { 10, 20, 30, 40, 50 };

            var result = strings
                .BeginTransform(int.Parse, 3)
                .ThenTransform(x => x * 10)
                .EndTransform()
                .ToArray();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        [Timeout(1000)]
        public void FullTest_03()
        {
            var strings = new[] { "1", "2", "3", "4", "5" };
            var numbers = new[] { 1, 5, 10, 2, 10, 20, 3, 15, 30, 4, 20, 40, 5, 25, 50 };

            var result = strings
                .BeginTransform(int.Parse)
                .ThenTransformMany(x => new[] { 1, 5, 10 }.Select(y => x * y))
                .EndTransform()
                .ToArray();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        [Timeout(1000)]
        public void FullTest_04()
        {
            var strings = new[] { "1", "2", "3", "4", "5" };
            var numbers = new[] { 1, 5, 10, 2, 10, 20, 3, 15, 30, 4, 20, 40, 5, 25, 50 };

            var result = strings
                .BeginTransform(int.Parse, 3)
                .ThenTransformMany(x => new[] { 1, 5, 10 }.Select(y => x * y), 5)
                .EndTransform()
                .ToArray();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        [Timeout(1000)]
        [ExpectedException(typeof(AggregateException))]
        public void FullTest_05()
        {
            var strings = new[] { "1", "2", "3", "4", "5", "banana" };

            var result = strings
                .BeginTransform(int.Parse, 1)
                .ThenTransformMany(x => new[] { 1, 5, 10 }.Select(y => x * y), 1)
                .EndTransform()
                .ToArray();
        }

        [TestMethod]
        [Timeout(1000)]
        [ExpectedException(typeof(AggregateException))]
        public void FullTest_06()
        {
            var strings = new[] { "5", "10", "15", "0" };

            var result = strings
                .BeginTransform(int.Parse, 1)
                .ThenTransform(x => 5 / x, 1)
                .EndTransform()
                .ToArray();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task FullTest_07()
        {
            var strings = new[] { "1", "2", "3", "4", "5" };
            var numbers = new[] { 1, 5, 10, 2, 10, 20, 3, 15, 30, 4, 20, 40, 5, 25, 50 };

            var result = (await strings
                .BeginTransform(int.Parse, 3)
                .ThenTransformMany(x => new[] { 1, 5, 10 }.Select(y => x * y), 5)
                .EndTransformAsync())
                .ToArray();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        [Timeout(1000)]
        public void FullTest_08()
        {
            var chars = new char[1];
            var strings = new[] { "1", "2", "3", "4", "5" };

            strings
                .BeginTransformVoid(x => x.CopyTo(0, chars, 0, 1))
                .EndTransformVoid();
        }

        [TestMethod]
        [Timeout(1000)]
        public void FullTest_09()
        {
            var chars = new char[1];
            var strings = new[] { "1", "2", "3", "4", "5" };

            strings
                .BeginTransformVoid(x => x.CopyTo(0, chars, 0, 1), 3)
                .EndTransformVoid();
        }

        [TestMethod]
        [Timeout(1000)]
        public void FullTest_10()
        {
            var chars = new char[2];
            var strings = new[] { "1", "2", "3", "4", "5" };

            strings
                .BeginTransform(x => x + "0")
                .ThenTransformVoid(x => x.CopyTo(0, chars, 0, 2))
                .EndTransformVoid();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task FullTest_11()
        {
            var chars = new char[2];
            var strings = new[] { "1", "2", "3", "4", "5" };

            await strings
                .BeginTransform(x => x + "0")
                .ThenTransformVoid(x => x.CopyTo(0, chars, 0, 2), 3)
                .EndTransformVoidAsync();
        }
    }
}