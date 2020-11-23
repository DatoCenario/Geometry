using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System;

namespace Geometry
{
    public class CircleCircuitNode : CircuitNodeBase
    {
        public float Raduis { get; }
        public int Frequency { get; }
        public Pivot Pivot { get; }
        public CircleCircuitNode(Pivot pivot, float radius, int frequency) : base()
        {
            Pivot = pivot;
            Raduis = radius;
            Frequency = frequency;
        }

        protected override IEnumerable<Vector3> VerticesProvider()
        {
            var pointer = Vector3.UnitX * Raduis;
            var deltaAngle = (float)Math.PI * 2 / Frequency;

            for (int i = 0; i < Frequency; i++)
            {
                yield return Pivot.ToGlobalCoords(pointer);
                pointer = pointer.Rotate(deltaAngle, AxisType.ZAxis);
            }
        }
    }
}