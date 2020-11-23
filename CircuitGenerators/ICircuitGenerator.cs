using System.Collections.Generic;

namespace Geometry
{
    public interface ICircuitGenerator
    {
        IEnumerable<Poly> Generate();
    }
}