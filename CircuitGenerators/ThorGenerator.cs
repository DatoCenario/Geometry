using System.Collections.Generic;
using System.Numerics;
using System;

namespace Geometry
{
    public class ThorGenerator : CircuitGeneratorBase
    {
        public int FrequencyFirst { get; set; }
        public int FrequencySecond { get; set; }
        public float RadiusFirst { get; set; }
        public float RadiusSecond { get; set; }
        public Vector3 Center { get; set; }

        public ThorGenerator(Vector3 center, float radiusFirst, float radiusSecond, int frequencyFirst
            , int frequencySecond)
        {
            Center = center;
            RadiusFirst = radiusFirst;
            RadiusSecond = radiusSecond;
            FrequencyFirst = frequencyFirst;
            FrequencySecond = frequencySecond;
        }

        public override IEnumerable<CircuitNodeBase> CircuitGraphProvider()
        {
            var pivot = Pivot.BasePivot(Center);

            var deltaAngle = (float)Math.PI * 2 / FrequencyFirst;

            var newPivot = pivot.Clone();
            newPivot.Move(newPivot.XAxis * RadiusFirst);
            var firstNode = new CircleCircuitNode(newPivot, RadiusSecond, FrequencySecond);

            var previousNode = firstNode;

            for (int i = 0; i < FrequencyFirst; i++)
            {
                pivot.Rotate(deltaAngle, AxisType.YAxis);
                newPivot = pivot.Clone();
                newPivot.Move(newPivot.XAxis * RadiusFirst);

                var newNode = new CircleCircuitNode(newPivot, RadiusSecond, FrequencySecond);
                previousNode.ConnectToNode(newNode);
                previousNode = newNode;
            }

            yield return firstNode;
        }
    }
}