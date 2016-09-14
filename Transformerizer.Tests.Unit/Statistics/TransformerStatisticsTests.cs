using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transformerizer.Statistics;

namespace Transformerizer.Tests.Unit.Statistics
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TransformerStatisticsTests
    {
        [TestMethod]
        public void Constructor_Works_When_No_Transforms_Occurred()
        {
            var statistics = new IThreadStatistics[]
            {
                new ThreadStatistics()
            };

            var result = new TransformerStatistics(statistics, null);
        }
    }
}
