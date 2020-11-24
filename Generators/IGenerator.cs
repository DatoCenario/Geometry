using System.Collections.Generic;

namespace Geometry
{
    public interface IGenerator
    {
        IEnumerable<Poly> Generate();
    }
}