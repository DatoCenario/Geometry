using System.Numerics;

namespace Geometry
{
    public interface IDynamicPrimitive
    {
        Pivot Pivot { get; }

        void Rotate(float angle, AxisType axisType);

        void Move(Vector3 vector);

        void Scale(float coef);
    }
}