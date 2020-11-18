using System.Numerics;

namespace Geometry
{
    public static class VectorExtensions
    {
        public static string Print(this Vector3 vector)
        {
            return $"{vector.X.ToString("F10")} {vector.Y.ToString("F10")} {vector.Z.ToString("F10")}";
        }

        public static string Print(this Vector2 vector)
        {
            return $"{vector.X.ToString("F10")} {vector.Y.ToString("F10")}";
        }
    }
}