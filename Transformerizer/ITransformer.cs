using System;
using System.Threading.Tasks;

namespace Transformerizer
{
    public interface ITransformer : IDisposable
    {
        /// <summary>
        /// The number of threads used to process this transformation.
        /// </summary>
        int ThreadCount { get; }

        /// <summary>
        /// Begins executing the transformation (and any dependent trasnformations) and returns a <see cref="Task"/> that can be waited on for completion of the transformation.
        /// </summary>
        Task ExecuteAsync();
    }
}
