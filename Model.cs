using System.Numerics;

namespace Geometry
{
    public class Model : ModelBase
    {
        public Model()
        {}

        public Model(Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            int[] indexes,
            int[] normalsIndexes,
            int[] textureIndexes,
            Pivot pivot,
            ITexture texture) : 
                base(vertices,normals,
                     texCoords,indexes,
                     normalsIndexes,
                     textureIndexes, 
                     pivot, 
                     texture)
            {}

        public Model(Vector3 center, Vector3[] vertices, int[] indexes) 
            : base(center, vertices, indexes)
        {}
    }
}