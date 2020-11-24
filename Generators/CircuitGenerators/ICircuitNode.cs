using System.Collections.Generic;
using System.Numerics;

namespace Geometry
{
    public interface ICircuitNode
    {
        public IEnumerable<ICircuitNode> ConectedNodes { get; }
        public void ConnectToNode(ICircuitNode other);
        public void DisconnectNode(ICircuitNode other);
        public IEnumerable<Poly> GetConnectionPolys(ICircuitNode other);
        public IEnumerable<Vector3> Vertices { get; }
    }
}