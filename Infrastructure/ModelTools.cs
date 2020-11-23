using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Geometry
{
    public static class ModelTools
    {
        static readonly float RayLength = float.MaxValue;
        public static bool IsPointInsideModel(Vector3 point, Model model)
        {
            var rayEnd = point + Vector3.UnitX * RayLength;

            int intersectsCount = CalculateIntersectsWithModel(point, rayEnd, model);

            return intersectsCount % 2 == 0 ? false : true;
        }

        public static int CalculateIntersectsWithModel(Vector3 begin, Vector3 end, Model model)
        {
            return model.GetPolys()
                .Where(p => VectorMath.AreIntersecting(p, begin, end, out var intersect))
                .Count();
        }

        //This exactly not a convex hull, here big mistake made
        public static Model GetModelConvexHull(Vector3[] vertices)
        {
            if(vertices.Length < 3) throw new ArgumentException("Minimal one poly requires.");

            //Making nullable
            var castedVertices = vertices.Cast<Vector3?>().ToArray();
            var first = castedVertices[0];

            var second = castedVertices
                .FirstOrDefault(v => v != vertices[0]);
            
            if(second == null) throw new ArgumentException("All points are same.");

            var length = (first - second).Value.Length();

            var third = castedVertices
                .FirstOrDefault(v => Math.Abs((first - v).Value.Length() + (second - v).Value.Length() - length) > 1);

            if(third == null) throw new ArgumentException("All points are in common line.");

            var poly1 = new Poly(first.Value, second.Value, third.Value);

            var fourth = castedVertices
                .FirstOrDefault(v => !VectorMath.BelongsPoly(poly1, v.Value));

            if(fourth == null) return MakeModelFromPolys(first.Value, new List<Poly>() {poly1});

            var poly2 = new Poly(first.Value, second.Value, fourth.Value);
            var poly3 = new Poly(first.Value, third.Value, fourth.Value);
            var poly4 = new Poly(second.Value, third.Value, fourth.Value);

            var polys = new HashSet<Poly>() { poly1, poly2, poly3, poly4};

            //Find inner point of tetraedr
            var v12 = first + (second - first) / 2;
            var v13 = first + (third - first) / 2;
            var v14 = first + (fourth - first) / 2;

            var v1213 = v12 + (v13 - v12) / 2;
            var v1214 = v12 + (v14 - v12) / 2;

            var innerPoint = (v1213 + (v1214 - v1213) / 2).Value;
            
            foreach (var v in vertices)
            {
                Poly? intersectPoly = null;
                foreach(var poly in polys)
                {
                    if(VectorMath.AreIntersecting(poly, innerPoint, v, out var intersect))
                    {
                        intersectPoly = poly;
                        break;
                    }
                }

                if (intersectPoly != null)
                {
                    polys.Remove(intersectPoly.Value);
                    polys.Add(new Poly(v, intersectPoly.Value.Point1, intersectPoly.Value.Point2));
                    polys.Add(new Poly(v, intersectPoly.Value.Point2, intersectPoly.Value.Point3));
                    polys.Add(new Poly(v, intersectPoly.Value.Point1, intersectPoly.Value.Point3));
                }
            }

            return MakeModelFromPolys(innerPoint ,polys.ToList());
        }

        public static Model MakeModelFromPolys(Vector3 center, List<Poly> polys)
        {
            var vertex_index = new Dictionary<Vector3, int>();
            var vertices = new List<Vector3>();
            var indexes = new List<int>();

            foreach (var poly in polys)
            {
                InsertVertexHelper(poly.Point1, vertex_index, vertices, indexes);
                InsertVertexHelper(poly.Point2, vertex_index, vertices, indexes);
                InsertVertexHelper(poly.Point3, vertex_index, vertices, indexes);
            }

            return new Model(center, vertices.ToArray(), indexes.ToArray());
        }

        private static void InsertVertexHelper(Vector3 vertex, Dictionary<Vector3, int> vertex_index,
             List<Vector3> vertices, List<int> indexes)
        {
            if (!vertex_index.TryGetValue(vertex, out var index))
            {
                index = vertices.Count;
                vertices.Add(vertex);
                vertex_index[vertex] = index;
            }
            indexes.Add(index);
        }
    }
}