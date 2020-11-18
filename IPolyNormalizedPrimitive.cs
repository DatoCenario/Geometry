using System.Collections.Generic;
using System.Numerics;

namespace Geometry
{
    public interface IPolyNormalizedPrimitive
    {
        IEnumerable<Vector3> Normals { get; }

        IEnumerable<int> NormalsIndexes { get; }

        int NormalsCount { get; }

        int NormalsIndexesCount { get; }
    }
}