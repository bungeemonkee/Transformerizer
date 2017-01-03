using System;

namespace Transformerizer.Transformers
{
    /// <summary>
    /// An <see cref="Exception"/> used to bubble up information about problems during transformation.
    /// </summary>
    public class TransformationException<TConsume> : Exception
    {
        /// <summary>
        /// The item being consumed when this exception occurred.
        /// </summary>
        public TConsume Consume { get; }

        /// <summary>
        /// Construct a new TransformationException that indicated which item was being transformed when an exception occurred.
        /// </summary>
        /// <param name="consume">The item being consumed when the exception occurred.</param>
        /// <param name="innerException">The inner exception.</param>
        public TransformationException(TConsume consume, Exception innerException)
            : base($"Exception occurred during transformation of input: {consume}", innerException)
        {
            Consume = consume;
        }
    }
}
