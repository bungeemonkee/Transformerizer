using System;
using System.Threading.Tasks;

namespace Transformerizer
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
        ///     Begins executing the transformation (and any dependent transformations) and returns a <see cref="Task" /> that can
        ///     be waited on for completion of the transformation.
        /// </summary>
        Task ExecuteAsync();
    }
}