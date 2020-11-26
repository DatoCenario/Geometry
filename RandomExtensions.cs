using System;

namespace Geometry
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            if(min > max || min < 0 || max < 0)
                throw new ArgumentException("Invalid boundaries.");

            float delta = max - min;
            return (float)random.NextDouble() * delta + min;
        }
    }
}