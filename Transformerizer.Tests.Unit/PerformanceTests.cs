using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Transformerizer.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PerformanceTests
    {
        [TestMethod]
        public void PerformanceTest_01()
        {
            const int parallelCount = 10;
            const long max = 10000000000000000L;

            var fibonacci1 = new Fibonacci(max);
            var fibonacci2 = new Fibonacci(max);

            var stopwatch = Stopwatch.StartNew();

            var result1 = new ConcurrentBag<Tuple<long, bool>>();
            Parallel.ForEach(fibonacci1, new ParallelOptions {MaxDegreeOfParallelism = 10}, x =>
            {
                var result = ComputeIsPrime(x);
                result1.Add(result);
            });
            var time1 = stopwatch.ElapsedTicks;

            stopwatch.Restart();

            var result2 = fibonacci2
                .BeginTransform(ComputeIsPrime, parallelCount)
                .EndTransform();

            var time2 = stopwatch.ElapsedTicks;

            var difference = time1 - time2;
            var percent = difference/((time1 + time2)/2D)*100D;
            Trace.WriteLine($"PerformanceTest_01 timing (in ticks) - Parallel.ForEach: {time1} Transformerizer: {time2} Difference: {difference} ({percent:##.00}%)");

            Assert.AreEqual(result1.Count, result2.Count);
            Assert.IsTrue(time2 < time1);
        }

        [Ignore]
        private static Tuple<long, bool> ComputeIsPrime(long value)
        {
            var root = (long) Math.Ceiling(Math.Sqrt(value));

            var isPrime = new LongRange(1L, root - 1L)
                .Count(x => value%x == 0L) == 1;

            return new Tuple<long, bool>(value, isPrime);
        }
    }
}