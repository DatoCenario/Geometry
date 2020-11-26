using System.Collections.Generic;
using System.Numerics;

namespace Geometry
{
    public class PyramidGenerator : IGenerator
    {
        public float Height { get; }
        public float SideLength { get; }
        public Vector3 Anchor { get; }
        private float _halfSide => SideLength / 2;

        public PyramidGenerator(Vector3 anchor, float sideLength, float height)
        {
            Anchor = anchor;
            SideLength = sideLength;
            Height = height;
        }

        public IEnumerable<Poly> Generate()
        {
            var baseSource = Anchor - new Vector3(0, Height, 0);

            var vertex1 = baseSource + new Vector3(_halfSide, 0, _halfSide);
            var vertex2 = baseSource + new Vector3(_halfSide, 0, -_halfSide);
            var vertex3 = baseSource + new Vector3(-_halfSide, 0, _halfSide);
            var vertex4 = baseSource + new Vector3(-_halfSide, 0, -_halfSide);

            yield return new Poly(Anchor, vertex1, vertex3);
            yield return new Poly(Anchor, vertex1, vertex2);
            yield return new Poly(Anchor, vertex3, vertex4);
            yield return new Poly(Anchor, vertex2, vertex4);
            yield return new Poly(vertex1, vertex2, vertex3);
            yield return new Poly(vertex2, vertex3, vertex4);
        }
    }
}