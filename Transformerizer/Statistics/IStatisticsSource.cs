namespace Transformerizer.Statistics
{
    /// <summary>
    ///     Defines anything that can provide transformer statistics.
    /// </summary>
    public interface IStatisticsSource
    {
        /// <summary>
        ///     Get the trasnformer statistics.
        /// </summary>
        /// <returns>An <see cref="ITransformerStatistics" /> instance.</returns>
        ITransformerStatistics GetStatistics();
    }
}