namespace Geometry
{
    public class EmptyTexture : ITexture
    {
        public Color Color;
        public Color GetPixel(int top, int left)
        {
            return Color;
        }

        public EmptyTexture(Color color)
        {
            Color = color;
        }
    }
}