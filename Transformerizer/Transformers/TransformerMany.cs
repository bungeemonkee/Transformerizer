using System.Collections.Generic;
using System.Linq;
using Transformerizer.Collections;
using Transformerizer.Methods;

namespace Transformerizer.Transformers
{
    /// <summary>
    ///     An <see cref="ITransformer{TProduce,TConsume}" /> that processes <see cref="TransformMany{TProduce,TConsume}" />
    ///     delegates.
    /// </summary>
    /// <typeparam name="TProduce">The type of the items produced.</typeparam>
    /// <typeparam name="TConsume">The type of the items consumed.</typeparam>
    public class TransformerMany<TProduce, TConsume> : TransformerBase<TProduce, TConsume>
    {
        private readonly TransformMany<TProduce, TConsume> _transform;

        /// <summary>
        ///     Creates a new TransformerMany instance.
        /// </summary>
        public TransformerMany(IBlockingQueueRead<TConsume> consume, TransformMany<TProduce, TConsume> transform)
            : this(consume, transform, null, DefaultThreadCount)
        {
        }

        /// <summary>
        ///     Creates a new TransformerMany instance.
        /// </summary>
        public TransformerMany(IBlockingQueueRead<TConsume> consume, TransformMany<TProduce, TConsume> transform, int threads)
            : this(consume, transform, null, threads)
        {
        }

        /// <summary>
        ///     Creates a new TransformerMany instance.
        /// </summary>
        public TransformerMany(IBlockingQueueRead<TConsume> consume, TransformMany<TProduce, TConsume> transform, ITransformer dependentTransformer)
            : this(consume, transform, dependentTransformer, dependentTransformer.ThreadCount)
        {
        }

        /// <summary>
        ///     Creates a new TransformerMany instance.
        /// </summary>
        public TransformerMany(IBlockingQueueRead<TConsume> consume, TransformMany<TProduce, TConsume> transform, ITransformer dependentTransformer, int threads)
            : base(consume, dependentTransformer, threads)
        {
            _transform = transform;
        }

        /// <summary>
        ///     See <see cref="TransformerBase{TConsume}.ProcessConsume(TConsume)" />.
        /// </summary>
        protected override void ProcessConsume(TConsume consume)
        {
            // Do the transformation
            var results = _transform(consume);

            // If nulls are not being preserved then remove any null values
            if (!PreserveNulls)
            {
                results = results.Where(x => x != null);
            }

            // Convert the results to some kind of collection (but they probably already are)
            var list = results as ICollection<TProduce> ?? results.ToList();

            // If there is anything to add then add it
            if (list.Count > 0)
            {
                Produce.TryAdd(list);
            }
        }
    }
}