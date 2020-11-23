using System.Numerics;

namespace Geometry
{
    public struct Poly
    {
        public Vector3 Point1;
        public Vector3 Point2;
        public Vector3 Point3;

        public Poly(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;
        }
    }
}