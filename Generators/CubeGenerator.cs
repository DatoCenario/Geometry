using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace Geometry
{
    public class CubeGenerator : IGenerator
    {
        public float SideLength { get; }
        public Vector3 Center { get; }

        public CubeGenerator(Vector3 center, float sideLength)
        {
            Center = center;
            SideLength = sideLength;
        }

        public IEnumerable<Poly> Generate()
        {
            var d = SideLength / 2;
            var deltas = new[] { -1, 1 };

            var vertices = deltas.SelectMany(d1 =>
                deltas.SelectMany(d2 =>
                    deltas.Select(d3 => Center + new Vector3(d1 * d, d2 * d, d3 * d))))
                    .ToArray();

            var continuePairs = new int[][]
            {
                new int [] { 0, 1, 2 },
                new int [] { 4, 5, 6 },
                new int [] { 0, 1, 4 },
                new int [] { 2, 3, 6 },
            };

            foreach (var pair in continuePairs)
            {
                yield return new Poly(vertices[pair[0]], vertices[pair[1]], vertices[pair[2]]);
                yield return new Poly(vertices[pair[1]], vertices[pair[2]], vertices[pair[2] + 1]);
            }

            var incrementalPairs = new int[][]
            {
                new int[] { 1, 7, 3 },
                new int[] { 0, 6, 2 }
            };

            foreach (var pair in incrementalPairs)
            {
                yield return new Poly(vertices[pair[0]], vertices[pair[1]], vertices[pair[2]]);
                yield return new Poly(vertices[pair[0]], vertices[pair[1]], vertices[pair[2] + 2]);
            }
        }
    }
}