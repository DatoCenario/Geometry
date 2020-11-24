using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Geometry
{
    public class SerpinskyCubeRecursiveHandler : IRecursiveHandler
    {
        public Vector3 Center { get; }
        public float SideLength { get; }
        public int RecursiveDepth { get; }
        private float _forthSide => SideLength / 4;

        public SerpinskyCubeRecursiveHandler(Vector3 center, float sideLength, int recursiveDepth)
        {
            Center = center;
            SideLength = sideLength;
            RecursiveDepth = recursiveDepth;
        }

        public IEnumerable<IRecursiveHandler> HandlersProvider()
        {
            var deltas = new int[] { -1, 1 };

            return deltas.SelectMany(d1 =>
                deltas.SelectMany(d2 =>
                    deltas.Select(d3 => new SerpinskyCubeRecursiveHandler(
                    Center + new Vector3(d1 * _forthSide, d2 * _forthSide, d3 * _forthSide),
                    SideLength / 2.2f, RecursiveDepth + 1))))
                    .ToArray();
        }

        public IEnumerable<Poly> PolysProvider()
        {
            var cubeGen = new CubeGenerator(Center, SideLength);
            return cubeGen.Generate();
        }
    }
}