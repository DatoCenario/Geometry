using System.Collections.Generic;
using System.Linq;

namespace Geometry
{
    public abstract class CircuitGeneratorBase : ICircuitGenerator
    {
        public IEnumerable<Poly> Generate()
        {
            var nodes = CircuitGraphProvider();

            return nodes.SelectMany(n => n.DeployCircuitGraphConnections());
        }

        public abstract IEnumerable<CircuitNodeBase> CircuitGraphProvider();
    }
}