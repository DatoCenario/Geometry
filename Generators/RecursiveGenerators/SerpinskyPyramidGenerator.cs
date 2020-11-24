using System.Numerics;
using System.Collections.Generic;

namespace Geometry
{
    public class SerpinskyPyramidGenerator : RecursiveGeneratorBase
    {
        public int RecursiveDepth { get; set; }
        public float SideLength { get; set; }
        public float Height { get; set; }
        public Vector3 Anchor { get; set; }
        public override IRecursiveHandler RecursiveHandler => new SerpinskyPyramidRecursiveHandler(Anchor, Height, SideLength, 0);

        public SerpinskyPyramidGenerator(Vector3 anchor, int recursiveDepth, float sideLength,
            float height) : base(recursiveDepth)
        {
            Anchor = anchor;
            SideLength = sideLength;
            Height = height;
        }
    }
}