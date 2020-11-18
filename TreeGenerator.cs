using System.Numerics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Geometry
{
    public static class TreeGenerator
    {
        class CircuitNodeData
        {
            public CircuitNodeData Previous { get; }
            public Pivot Pivot { get; }
            public float Radius { get; }
            public int Depth { get; }
            public int Frequency { get; }
            public float Length { get; }

            public CircuitNodeData(Pivot pivot, float radius, CircuitNodeData previous, int frequency, 
                float length)
            {
                if(previous == null) Depth = 1;
                else Depth = previous.Depth + 1;

                if(pivot == null) throw new ArgumentException(nameof(pivot));

                Previous = previous;
                Pivot = pivot;
                Radius = radius;
                Frequency = frequency;
                Length = length;
            }

            public IEnumerable<Vector3> GenerateVertices()
            {
                var pointer = Vector3.UnitX * Radius;

                var deltaAngle = (float)(Math.PI * 2 / Frequency);

                for (int i = 0; i < Frequency; i++)
                {
                    yield return Pivot.ToGlobalCoords(pointer);
                    pointer = pointer.Rotate(deltaAngle, AxisType.ZAxis);
                }
            }
        }
        static Random random = new Random(DateTime.Now.Millisecond);
        static List<Poly> polys;

        public static Model GenerateTree(Vector3 root, float initialRadius, float initialLength,
             int branchesCount, int frequency)
        {
            int total = branchesCount;

            polys = new List<Poly>();
            var queue = new Queue<CircuitNodeData>();

            var rootNode = new CircuitNodeData(Pivot.BasePivot(root), 
                initialRadius, null, frequency, initialLength);

            queue.Enqueue(rootNode);
            branchesCount--;

            while (branchesCount != 0)
            {
                var current = queue.Dequeue();

                if(current.Previous != null) ConnectNodes(current, current.Previous);

                int count = Math.Min(random.Next(1, 5), branchesCount);
                for (int i = 0; i < count; i++)
                {
                    branchesCount--;

                    var pivot = current.Pivot.Clone();
                    RotateRandom(pivot);
                    pivot.Move(current.Length * pivot.ZAxis);

                    queue.Enqueue(new CircuitNodeData(pivot, current.Radius / 1.5f,
                        current, current.Frequency, current.Length / 1.2f));
                }
            }

            return ModelTools.MakeModelFromPolys(Vector3.Zero, polys);
        }

        private static void ConnectNodes(CircuitNodeData node1, CircuitNodeData node2)
        {
            //Now it pretty simple (fix later)
            if(node1.Frequency != node2.Frequency)
                throw new InvalidOperationException("Difference in vertices lengths");

            var verticesFirst = node1.GenerateVertices().ToList();
            var verticesSecond = node2.GenerateVertices().ToList();

            Poly poly1, poly2;

            for (int i = 0; i < node1.Frequency - 1; i++)
            {
                poly1 = new Poly(verticesFirst[i], verticesSecond[i], verticesSecond[i + 1]);
                poly2 = new Poly(verticesFirst[i], verticesFirst[i + 1], verticesSecond[i + 1]);

                polys.Add(poly1);
                polys.Add(poly2);
            }

            poly1 = new Poly(verticesFirst[node1.Frequency - 1], verticesSecond[node1.Frequency - 1], verticesSecond[0]);
            poly2 = new Poly(verticesFirst[node1.Frequency - 1], verticesFirst[0], verticesSecond[0]);

            polys.Add(poly1);
            polys.Add(poly2);
        }
        private static void RotateRandom(Pivot pivot)
        {
            var xAngle = GetRandomAngle();
            var yAngle = GetRandomAngle();
            var zAngle = GetRandomAngle();

            pivot.Rotate(xAngle, AxisType.XAxis);
            pivot.Rotate(yAngle, AxisType.YAxis);
            pivot.Rotate(zAngle, AxisType.ZAxis);
        }

        static float RotationCoef = 6;
        static float PiCoef = (float)Math.PI / RotationCoef;
        private static float GetRandomAngle()
        {
            var randNum = random.NextDouble();
            var angle = (float)(PiCoef * (randNum * 2 - 1));
            return angle;
        }
    }
}