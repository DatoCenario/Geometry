using System.Numerics;
using System.Linq;

namespace Geometry
{
    public class Cube : ModelBase
    {
        public float SideLength { get; }

        public Cube(Vector3 center, float sideLength)
        {
            Pivot = Pivot.BasePivot(center);

            var delta = new float[] { -sideLength / 2, sideLength / 2 };

            _vertices = delta
                .SelectMany(n => delta
                .SelectMany(n1 => delta.Select(n2 => new Vector3(n, n1, n2))))
                .ToArray();

            UpdateGlobalVertices();
            
            _indexes = new int[]
                {
                    1,3,2,
                    1,0,2,
                    1,0,4,
                    1,5,4,
                    5,6,7,
                    5,6,4,
                    3,6,7,
                    3,6,2,
                    1,3,7,
                    1,5,7,
                    2,0,6,
                    4,0,6
                };

            CreateModelNormals();
        }
    }
}