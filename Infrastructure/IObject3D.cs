using System.Numerics;

namespace Geometry
{
    public interface IObject3D
    {
        void Move(Vector3 vector);
        void Rotate(float angle, AxisType axisType);
    }
}