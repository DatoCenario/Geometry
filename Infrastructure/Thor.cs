using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Geometry
{
    public class Thor : ModelBase
    {
        public float FirstRadius { get; }
        public float SecondRadius { get; }
        public int FirstFrequency { get; }
        public int SecondFrequency { get; }
        public Thor(Vector3 center, float radius1, float radius2, int firstFrequency, int secondFrequency) : base()
        {
            FirstRadius = radius1;
            SecondRadius = radius2;
            FirstFrequency = firstFrequency;
            SecondFrequency = secondFrequency;

            Pivot = Pivot.BasePivot(center);
            _vertices = new Vector3[FirstFrequency * SecondFrequency];
            _indexes = new int[FirstFrequency * SecondFrequency * 6];

            var step1 = (float)Math.PI * 2 / FirstFrequency;
            var step2 = (float)Math.PI * 2 / SecondFrequency;
            var rPivot1 = Pivot.BasePivot(Vector3.Zero);

            for (int i = 0; i < FirstFrequency; i++)
            {
                var rPivot2 = rPivot1.Clone();
                rPivot2.Move(rPivot2.XAxis * radius1);

                var pointer = Vector3.UnitX * radius2;

                for (int g = 0; g < SecondFrequency; g++)
                {
                    _vertices[i * SecondFrequency + g] = rPivot2.ToGlobalCoords(pointer);
                    pointer = pointer.Rotate(step2 , AxisType.ZAxis);
                }

                rPivot1.Rotate(step1, AxisType.YAxis);
            }

            //Generation of thor indexes
            int k = 0;

            for (int i = 0; i < FirstFrequency - 1; i++)
            {
                for (int g = 0; g < SecondFrequency - 1; g++)
                {
                    ConnectFragment(i, g, i + 1, g + 1, k);
                    k += 6;
                }

                ConnectFragment(i, SecondFrequency - 1, i + 1, 0, k);
                k += 6;
            }

            for (int g = 0; g < SecondFrequency - 1; g++)
            {
                ConnectFragment(FirstFrequency - 1, g, 0, g + 1, k);
                k += 6;
            }

            ConnectFragment(FirstFrequency - 1, SecondFrequency - 1, 0, 0, k);
            k += 6;

            UpdateGlobalVertices();

            CreateModelNormals();
        }

        private void ConnectFragment(int i1, int g1, int i2, int g2, int pos)
        {
            //int pos = i1 * SecondFrequency + g1;

            _indexes[pos] = i1 * SecondFrequency + g1;
            _indexes[pos + 1] = i1 * SecondFrequency + g2;
            _indexes[pos + 2] = i2 * SecondFrequency + g2;
            _indexes[pos + 3] = i1 * SecondFrequency + g1;
            _indexes[pos + 4] = i2 * SecondFrequency + g1;
            _indexes[pos + 5] = i2 * SecondFrequency + g2;
        }
    }
}
