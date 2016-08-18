using System;
using System.Threading.Tasks;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     Defines the basic, non-generic, transformer functionality.
    /// </summary>
    public interface ITransformer : IDisposable
    {
        /// <summary>
        ///     The number of threads used to process this transformation.
        /// </summary>
        int ThreadCount { get; }

        /// <summary>
        ///     By default when a transform function returns a null value it will not be copied into the next buffer.
        ///     This allows that behavior to be overwritten.
        /// </summary>
        bool PreserveNulls { get; set; }

        /// <summary>
        ///     Begins executing the transformation (and any dependent transformations) and returns a <see cref="Task" /> that can
        ///     be waited on for completion of the transformation.
        /// </summary>
        Task ExecuteAsync();
    }
}