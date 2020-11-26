using System.Numerics;
using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Geometry
{
    static class VectorMath
    {
        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, -(v1.X * v2.Z - v1.Z * v2.X), v1.X * v2.Y - v1.Y * v2.X);
        }

        public static float Angle(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Acos(Dot(v1, v2) / (v1.Length() * v2.Length()));
        }

        public static float Angle(Vector2 v1, Vector2 v2)
        {
            return (float)Math.Acos(Dot(v1, v2) / (v1.Length() * v2.Length()));
        }

        public static Vector3 Rotate(this Vector3 Vector3, float angle, AxisType axis)
        {
            var rotation = axis == AxisType.XAxis ? 
                Matrix4x4.CreateRotationX(angle) : 
                axis == AxisType.YAxis ? 
                Matrix4x4.CreateRotationY(angle) :
                Matrix4x4.CreateRotationZ(angle);
            
            return Vector3.Transform(Vector3, rotation);
        }

        public static Vector3 Proection(Vector3 v1, Vector3 v2)
        {
            return Dot(v1, v2) / Dot(v2, v2) * v2;
        }

        public static Vector3 Transform(this Vector3 Vector3, Matrix4x4 matrix4X4)
        {
            var vec4 = new Vector4(Vector3.X, Vector3.Y, Vector3.Z, 1);
            var newVector = Vector4.Transform(vec4, matrix4X4);
            return new Vector3(newVector.X / newVector.W,
                               newVector.Y / newVector.W, 
                               newVector.Z / newVector.W);
        }

        public static float[] GetLineCoefs(Vector3 v1, Vector3 v2)
        {
            var d = v2 - v1;
            if (d.X != 0 && d.Y != 0 && d.Z != 0)
            {
                var f = d.Y * d.Z; var s = -2 * d.X * d.Z; var t = d.Y * d.Z;
                return new float[] { f, s, t, -f * v1.X + s * v1.Y - t * v1.Z };
            }
            if (d.X == 0)
            {
                return new float[] { 0, d.Z, -d.Y, -d.Z * v1.Y + v1.Z * d.Y };
            }
            else if (d.Y == 0)
            {
                return new float[] { d.Z, -d.X, 0, -d.Z * v1.X + v1.Z * d.X };
            }
            else if (d.Z == 0)
            {
                return new float[] { d.Y, -d.X, 0, -d.Y * v1.X + v1.Y * d.X };
            }
            else
            {
                return new float[] { 0, 0, 0, 0 };
            }
        }

        public static float[] GetSurfaceCoefs(Poly poly)
        {
            var v13 = poly.Point3 - poly.Point1;
            var v12 = poly.Point2 - poly.Point1;

            var cross = Cross(v13, v12);
            var dot = -Dot(cross, poly.Point1);
            return new float[] { cross.X, cross.Y, cross.Z, dot };
        }

        public static Vector3 GetNormal(Poly poly)
        {
            var v13 = poly.Point3 - poly.Point1;  
            var v12 = poly.Point2 - poly.Point1;  

            var cross = Cross(v12, v13);
            return Vector3.Normalize(cross);
        }

        public static bool LinesIntersect(Vector3 begin1, Vector3 end1, Vector3 begin2, Vector3 end2,
             out Vector3 intersect)
        {
            intersect = Vector3.Zero;

            var begin_end1 = end1 - begin1;
            var begin_end2 = end2 - begin2;

            //check that lines are not collinear
            if (Cross(begin_end1, begin_end2).Length() < 0.00001)
            {
                return false;
            }

            var coefs = GetLineCoefs(begin2, end2);
            var val1 = (coefs[0] * begin1.X + coefs[1] * begin1.Y + coefs[2] * begin1.Z + coefs[3]);
            var val2 = (coefs[0] * begin_end1.X + coefs[1] * begin_end1.Y + coefs[2] * begin_end1.Z);

            var scaleCoef = - val1 / val2;
            var contactPoint = begin1 + begin_end1 * scaleCoef;
            if (scaleCoef >= 0 && scaleCoef <= 1)
            {
                intersect = contactPoint;
                return true;
            }

            return false;
        }

        public static bool BelongsPoly(Poly poly, Vector3 point)
        {
            //Using area method to determine whetger a point belongs to poly
            var v1 = poly.Point1 - point;
            var v2 = poly.Point2 - point;
            var v3 = poly.Point3 - point;

            var v12 = poly.Point3 - poly.Point1;
            var v23 = poly.Point3 - poly.Point2;

            var s1 = Cross(v1, v2).Length() + Cross(v2, v3).Length() + Cross(v3, v1).Length();
            var s2 = Cross(v12, v23).Length();
            return Math.Abs(s1 - s2) < 0.01;
        }

        public static bool AreIntersecting(Poly poly, Vector3 begin, Vector3 end, out Vector3 intersect)
        {
            var coefs = GetSurfaceCoefs(poly);

            var begin_end = end - begin;

            var val1 = (coefs[0] * begin.X + coefs[1] * begin.Y + coefs[2] * begin.Z + coefs[3]);
            var val2 = (coefs[0] * begin_end.X + coefs[1] * begin_end.Y + coefs[2] * begin_end.Z);
            
            var scaleCoef = - val1 / val2;
            var contactPoint = begin + (scaleCoef * begin_end);

            if (scaleCoef >= 0 && scaleCoef <= 1 && BelongsPoly(poly, contactPoint))
            {
                intersect = contactPoint;
                return true; 
            }

            intersect = Vector3.Zero; 
            return false;
        }

        public static IEnumerable<Poly> TriangulateOrderedVertexSet(IEnumerable<Vector3> vertices)
        {
            if(vertices == null)
                throw new ArgumentException(nameof(vertices));

            var enumerator = vertices.GetEnumerator();

            if(!enumerator.MoveNext())
                yield break;

            var first = enumerator.Current;

            if(!enumerator.MoveNext())
                throw new ArgumentException($"Collection of one vertex cannot be triangulated {nameof(vertices)}");

            var previous = enumerator.Current;

            if(!enumerator.MoveNext())
                throw new ArgumentException($"Collection of two vertices cannot be triangulated {nameof(vertices)}");

            var current = enumerator.Current;

            yield return new Poly(first, previous, current);

            while(enumerator.MoveNext())
            {
                previous = current;
                current = enumerator.Current;
                yield return new Poly(first, previous, current);
            }
        }

        public static float[] GetLineCoefs(Vector2 begin, Vector2 end)
        {
            var a = end.Y - begin.Y;
            var b = begin.X - end.X;
            var c = begin.Y * end.X - begin.X * end.Y;
            return new float[] { a, b, c };
        }

        public static bool AreIntersecting(Vector2 begin1, Vector2 end1, Vector2 begin2, Vector2 end2,
            out Vector2 intersect)
        {
            var begin_end1 = Vector2.Normalize(end1 - begin1);
            var begin_end2 = Vector2.Normalize(end2 - begin2);

            var length1 = (begin_end1 - begin_end2).Length();
            var length2 = (begin_end1 + begin_end2).Length();

            if(length1 < 0.01 || length2 < 0.01) 
            {
                intersect = Vector2.Zero;
                return false;
            }

            var coefs = GetLineCoefs(begin2, end2);

            var val1 = (coefs[0] * begin1.X + coefs[1] * begin1.Y + coefs[2]);
            var val2 = (coefs[0] * begin_end1.X + coefs[1] * begin_end1.Y);

            var scaleCoef = -val1 / val2;
            intersect = begin1 + (scaleCoef * begin_end1);

            return true;
        }
    }
}
