using System.Numerics;
using System;
using System.Collections.Generic;

namespace Geometry
{
    public class TreeGenerator : CircuitGeneratorBase
    {
        public TreeConfiguration Configuration { get; set; }
        private int BranchinessTotal => (int)(Configuration.BranchesCount * Configuration.BranchinessPercent / 100);
        public  Vector3 RootPosition { get; }
        public  float _rotationCoeficient => (float)Math.PI * Configuration.FlatnessPercents / 100;
        public Random _random;

        public TreeGenerator(Vector3 start, TreeConfiguration configuration)
        {
            Configuration = configuration;
            RootPosition = start;
            _random = new Random();
        }

        public override IEnumerable<CircuitNodeBase> CircuitGraphProvider()
        {
            var initialPivot = Pivot.BasePivot(RootPosition);
            var rootNode = new CircleCircuitNode(initialPivot, Configuration.InitialRadius, 
                Configuration.InitialFrequency);

            var queue = new Queue<Tuple<CircleCircuitNode, float>>();
            queue.Enqueue(Tuple.Create(rootNode, Configuration.InitialLength));

            int branchesCountPassed = Configuration.BranchesCount;

            while(branchesCountPassed != 0)
            {
                var current = queue.Dequeue();
                var node = current.Item1;
                var length = current.Item2;


                int count = Math.Max(1, Math.Min(_random.Next(0, BranchinessTotal), branchesCountPassed));
                branchesCountPassed -= count;

                for (int i = 0; i < count; i++)
                {
                    var pivot = node.Pivot.Clone();
                    RotateRandom(pivot);
                    pivot.Move(pivot.ZAxis * length);

                    var newRadius = node.Raduis * Configuration.RadiusDecreasePercents / 100;
                    var newFrequency = Math.Max(3, (int)((float)node.Frequency * Configuration.FrequencyDecreasePercents / 100));

                    var newNode = new CircleCircuitNode(
                        pivot, 
                        newRadius,
                        newFrequency);

                    node.ConnectToNode(newNode);
                    queue.Enqueue(Tuple.Create(newNode, length * Configuration.LengthDecreasePercents / 100));
                }
            }

            yield return rootNode;
        }


        public  void RotateRandom(Pivot pivot)
        {
            var xAngle = GetRandomAngle();
            var yAngle = GetRandomAngle();
            var zAngle = GetRandomAngle();

            pivot.Rotate(xAngle, AxisType.XAxis);
            pivot.Rotate(yAngle, AxisType.YAxis);
            pivot.Rotate(zAngle, AxisType.ZAxis);
        }


        public  float GetRandomAngle()
        {
            var randFloat = (float)_random.NextDouble();
            var angle = _rotationCoeficient * (randFloat * 2 - 1);
            return angle;
        }
    }
}