using System.Collections.Generic;
using System.Numerics;

namespace Geometry
{
    public class SerpinskyPyramidRecursiveHandler : IRecursiveHandler
    {
        public Vector3 Anchor { get; }
        public float Height { get; }
        public float SideLength { get; }
        public int RecursiveDepth { get; }
        private float _halfSide => SideLength / 2;
        private float _halfHeight => Height / 2;

        public SerpinskyPyramidRecursiveHandler(Vector3 anchor, float height, float sideLength,
            int recursiveDepth)
        {
            Anchor = anchor;
            Height = height;
            SideLength = sideLength;
            RecursiveDepth = recursiveDepth;
        }

        public IEnumerable<IRecursiveHandler> HandlersProvider()
        {
            yield return new SerpinskyPyramidRecursiveHandler(Anchor, _halfHeight, _halfSide,
                RecursiveDepth + 1);

            yield return new SerpinskyPyramidRecursiveHandler(Anchor + new Vector3(_halfSide / 2, -_halfHeight, _halfSide / 2), 
                _halfHeight, _halfSide, RecursiveDepth + 1);

            yield return new SerpinskyPyramidRecursiveHandler(Anchor + new Vector3(_halfSide / 2, -_halfHeight, -_halfSide / 2),
                 _halfHeight, _halfSide, RecursiveDepth + 1);

            yield return new SerpinskyPyramidRecursiveHandler(Anchor + new Vector3(-_halfSide / 2, -_halfHeight, _halfSide / 2),
                _halfHeight, _halfSide, RecursiveDepth + 1);

            yield return new SerpinskyPyramidRecursiveHandler(Anchor + new Vector3(-_halfSide / 2, -_halfHeight, -_halfSide / 2),
                _halfHeight, _halfSide, RecursiveDepth + 1);
        }

        public IEnumerable<Poly> PolysProvider()
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