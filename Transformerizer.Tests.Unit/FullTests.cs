﻿using System;
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
        public void FullTest_01()
        {
            var strings = new[] {"1", "2", "3", "4", "5"};
            var numbers = new[] {10, 20, 30, 40, 50};

            var result = strings
                .BeginTransform(x => int.Parse(x), 1)
                .ThenTransform(x => x*10)
                .EndTransform();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        public void FullTest_02()
        {
            var strings = new[] {"1", "2", "3", "4", "5"};
            var numbers = new[] {10, 20, 30, 40, 50};

            var result = strings
                .BeginTransform(x => int.Parse(x), 3)
                .ThenTransform(x => x*10)
                .EndTransform();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        public void FullTest_03()
        {
            var strings = new[] {"1", "2", "3", "4", "5"};
            var numbers = new[] {1, 5, 10, 2, 10, 20, 3, 15, 30, 4, 20, 40, 5, 25, 50};

            var result = strings
                .BeginTransform(x => int.Parse(x))
                .ThenTransformMany(x => new[] {1, 5, 10}.Select(y => x*y))
                .EndTransform();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        public void FullTest_04()
        {
            var strings = new[] {"1", "2", "3", "4", "5"};
            var numbers = new[] {1, 5, 10, 2, 10, 20, 3, 15, 30, 4, 20, 40, 5, 25, 50};

            var result = strings
                .BeginTransform(x => int.Parse(x), 3)
                .ThenTransformMany(x => new[] {1, 5, 10}.Select(y => x*y), 5)
                .EndTransform();

            CollectionAssert.AreEquivalent(numbers, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void FullTest_05()
        {
            var strings = new[] {"1", "2", "3", "4", "5", "banana"};

            var result = strings
                .BeginTransform(x => int.Parse(x), 1)
                .ThenTransformMany(x => new[] {1, 5, 10}.Select(y => x*y), 1)
                .EndTransform();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void FullTest_06()
        {
            var strings = new[] {"5", "10", "15", "0"};

            var result = strings
                .BeginTransform(x => int.Parse(x), 1)
                .ThenTransform(x => 5/x, 1)
                .EndTransform();
        }

        [TestMethod]
        public async Task FullTest_07()
        {
            var strings = new[] {"1", "2", "3", "4", "5"};
            var numbers = new[] {1, 5, 10, 2, 10, 20, 3, 15, 30, 4, 20, 40, 5, 25, 50};

            var result = await strings
                .BeginTransform(x => int.Parse(x), 3)
                .ThenTransformMany(x => new[] {1, 5, 10}.Select(y => x*y), 5)
                .EndTransformAsync();

            CollectionAssert.AreEquivalent(numbers, result);
        }
    }
}