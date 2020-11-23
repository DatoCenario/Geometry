using System.Collections.Generic;
using System.Numerics;

namespace Geometry
{
    public interface IVertexPrimitive 
    {
        IEnumerable<Vector3> Vertices { get; }

        IEnumerable<Vector3> GlobalVertices { get; }

        IEnumerable<int> Indexes { get; }

        int VerticesCount { get; }

        int IndexesCount { get; }
    }
}