using System;

namespace Geometry
{
    public class TreeConfiguration
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

        public int InitialFrequency
        {
            get { return _initialFrequency; }
            set
            {
                _initialFrequency = Math.Max(0, value);
            }
        }
        public float InitialLength
        {
            get { return _initialLength; }
            set
            {
                _initialLength = Math.Max(0, value);
            }
        }
        public float InitialRadius
        {
            get { return _initialRadius; }
            set
            {
                _initialRadius = Math.Max(0, value);
            }
        }
        public int BranchesCount
        {
            get { return _branchesCount; }
            set
            {
                _branchesCount = Math.Max(0, value);
            }
        }
        public float FlatnessPercents
        {
            get { return _flatnessPercents; }
            set
            {
                _flatnessPercents = Math.Min(100, Math.Max(-100, value));
            }
        }
        public float FrequencyDecreasePercents
        {
            get { return _frequencyDecreasePercents; }
            set
            {
                _frequencyDecreasePercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public float LengthDecreasePercents
        {
            get { return _lengthDecreasePercents; }
            set
            {
                _lengthDecreasePercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public float RadiusDecreasePercents
        {
            get { return _radiusDecreasePercents; }
            set
            {
                _radiusDecreasePercents = Math.Min(100, Math.Max(0, value));
            }
        }
        public float BranchinessPercent
        {
            get { return _branchinessPercents; }
            set
            {
                _branchinessPercents = Math.Min(100, Math.Max(0, value));
            }
        }

        public TreeConfiguration(int initialFrequency, float initialLength, float initialRadius,
            int branchesCount, float frequencyDecreasePercents, float lengthDecreasePercents,
            float radiusDecreasePercents, float branchinessPercent, float flatnessPercents)
        {
            InitialFrequency = initialFrequency;
            InitialLength = initialLength;
            InitialRadius = initialRadius;
            BranchesCount = branchesCount;
            FrequencyDecreasePercents = frequencyDecreasePercents;
            LengthDecreasePercents = lengthDecreasePercents;
            RadiusDecreasePercents = radiusDecreasePercents;
            BranchinessPercent = branchinessPercent;
            FlatnessPercents = flatnessPercents;
        }

        public static TreeConfiguration KvazimodaTree =>
            new TreeConfiguration(100, 200, 70, 2000, 80, 80, 60, 0.2f, 40);

        public static TreeConfiguration SelfGrownTree =>
            new TreeConfiguration(100, 200, 70, 2000, 80, 80, 60, 0.2f, 90);

        public static TreeConfiguration HightTree =>
            new TreeConfiguration(100, 200, 70, 2000, 80, 80, 60, 0.2f, 10);

        public static TreeConfiguration BoldTree =>
            new TreeConfiguration(300, 200, 70, 2000, 50, 80, 60, 0.2f, 30);

        public static TreeConfiguration BigTree =>
            new TreeConfiguration(300, 300, 70, 50000, 50, 80, 60, 0.01f, 15);

        public static TreeConfiguration FirTree =>
            new TreeConfiguration(300, 300, 70, 50000, 50, 80, 60, 0.01f, -50);
    }
}