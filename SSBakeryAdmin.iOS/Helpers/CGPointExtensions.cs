using CoreGraphics;
using System;

namespace SSBakeryAdmin.iOS.Helpers
{
    public static class CGPointExtensions
    {
        public static CGPoint Floor(this CGPoint point)
        {
            point.X = NMath.Floor(point.X);
            point.Y = NMath.Floor(point.Y);

            return point;
        }
    }
}