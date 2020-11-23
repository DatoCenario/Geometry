using System.Collections.Generic;
using System.Numerics;

namespace Geometry
{
    public interface ITexturedPrimitive
    {
        ITexture Texture { get; }

        IEnumerable<Vector2> TextureCoords { get; }

        IEnumerable<int> TextureIndexes { get; }
    }
}