using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Geometry
{
    class Function : ModelBase
    {
        public Function(Vector3 center, Func<float, float, float> f, float xstart, float ystart, float xend, float yend, float step)
        {
            Pivot = Pivot.BasePivot(center);

            int width = (int)((xend - xstart) / step);
            int height = (int)((yend - ystart) / step);
            int len = width * height;

            _vertices = new Vector3[len];
            _globalVertices = new Vector3[len];
            _indexes = new int[width * height * 6];
            _normalsIndexes = new int[width * height * 6];
            _normals = new Vector3[width * height];

            float x = xstart, y = ystart;

            for (int i = 0; i < width; i++)
            {
                y = ystart;
                for (int g = 0; g < height; g++)
                {
                    int ind = g * width + i;
                    var local = _vertices[ind] = new Vector3(x, y, f.Invoke(x,y));
                    _globalVertices[ind] = Pivot.ToGlobalCoords(local);
                    y += step;
                }
                x += step;
            }


            for (int i = 0; i < width - 1; i++)
            {
                for (int g = 0; g < height - 1; g++)
                {
                    int k = (i * height + g) * 6;
                    int i1 = _indexes[k] = i * height + g;
                    int i2 = _indexes[k + 1] = i * height + g + 1;
                    int i3 = _indexes[k + 2] = (i + 1) * height + g;
                    int i4 = _indexes[k + 3] = i * height + g + 1;
                    int i5 = _indexes[k + 4] = (i + 1) * height + g + 1;
                    int i6 = _indexes[k + 5] = (i + 1) * height + g;
                }
            }

            CreateModelNormals();
        }
    }
}
