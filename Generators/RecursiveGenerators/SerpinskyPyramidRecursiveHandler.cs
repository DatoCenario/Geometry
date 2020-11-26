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
        private PyramidGenerator _generator => new PyramidGenerator(Anchor, SideLength, Height);

        public SerpinskyPyramidRecursiveHandler(Vector3 anchor, float height, float sideLength,
            int recursiveDepth)
        {
            Anchor = anchor;
            Height = height;
            SideLength = sideLength;
            RecursiveDepth = recursiveDepth;
        }

        public virtual IEnumerable<IRecursiveHandler> HandlersProvider()
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

        public virtual IEnumerable<Poly> PolysProvider()
        {
            return _generator.Generate();
        }
    }
}