using System.Collections.Generic;

namespace Transformerizer
{
    public delegate IEnumerable<TProduce> TransformMany<TProduce, TConsume>(TConsume consume);
}
