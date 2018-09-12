using System;

namespace MyLovely2dWife
{
    public static class MathHelper
    {
        public static float Clamp(float val, float min, float max) => Math.Min(max, Math.Max(min, val));
    }
}