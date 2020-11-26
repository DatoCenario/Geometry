using System.Numerics;
using System;
using System.Collections.Generic;

namespace Geometry
{
    public class TreeGenerator : CircuitGeneratorBase
    {
        private int _initialFrequency;
        private float _initialRadius;
        private float _initialLength;
        private int _branchesCount;
        private float _flatnessPercents;
        private float _branchinessPercents;
        private float _lengthDecreasePercents;
        private float _radiusDecreasePercents;
        private float _frequencyDecreasePercents;
        public  Random _random;
        public  int InitialFrequency
        {
            get { return _initialFrequency; }
            set
            {
                _initialFrequency = Math.Max(0, value);
            }
        }
        public  float InitialLength
        {
            get { return _initialLength; }
            set
            {
                _initialLength = Math.Max(0, value);
            }
        }
        public  float InitialRadius
        {
            get { return _initialRadius; }
            set
            {
                _initialRadius = Math.Max(0, value);
            }
        }
        public  int BranchesCount
        {
            get { return _branchesCount; }
            set
            {
                _branchesCount = Math.Max(0, value);
            }
        }
        public  float FlatnessPercents
        {
            get { return _flatnessPercents; }
            set
            {
                _flatnessPercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public  float FrequencyDecreasePercents
        {
            get { return _frequencyDecreasePercents; }
            set
            {
                _frequencyDecreasePercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public  float LengthDecreasePercents
        {
            get { return _lengthDecreasePercents; }
            set
            {
                _lengthDecreasePercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public  float RadiusDecreasePercents
        {
            get { return _radiusDecreasePercents; }
            set
            {
                _radiusDecreasePercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public  float BranchinessPercent
        {
            get { return _branchinessPercents; }
            set
            {
                 _branchinessPercents = Math.Min(100, Math.Max(0, value));
            }
        }
        private int BranchinessTotal => (int)(BranchesCount * BranchinessPercent / 100);
        public  Vector3 RootPosition { get; }
        public  float _rotationCoeficient => (float)Math.PI * FlatnessPercents / 100;

        public TreeGenerator(Vector3 start, int initialFrequency, float initialLength, 
            float initialRadius, float flatnessPercents, int branchesCount)
        {
            InitialFrequency = initialFrequency;
            InitialLength = initialLength;
            InitialRadius = initialRadius;
            FlatnessPercents = flatnessPercents;
            BranchesCount = branchesCount;
            RootPosition = start;
            _random = new Random();

            InitializeDefault();
        }

        private void InitializeDefault()
        {
            BranchinessPercent = 0.3f;
            FrequencyDecreasePercents = 90;
            RadiusDecreasePercents = 80;
            LengthDecreasePercents = 90;
        }

        public override IEnumerable<CircuitNodeBase> CircuitGraphProvider()
        {
            var initialPivot = Pivot.BasePivot(RootPosition);
            var rootNode = new CircleCircuitNode(initialPivot, InitialRadius, InitialFrequency);

            var queue = new Queue<Tuple<CircleCircuitNode, float>>();
            queue.Enqueue(Tuple.Create(rootNode, InitialLength));

            int branchesCountPassed = BranchesCount;

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

                    var newRadius = node.Raduis * RadiusDecreasePercents / 100;
                    var newFrequency = Math.Max(3, (int)((float)node.Frequency * FrequencyDecreasePercents / 100));

                    var newNode = new CircleCircuitNode(
                        pivot, 
                        newRadius,
                        newFrequency);

                    node.ConnectToNode(newNode);
                    queue.Enqueue(Tuple.Create(newNode, length * LengthDecreasePercents / 100));
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