using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    public static class VectorExtensions
    {
        public static Vector3 Rotate(this Vector3 start, double angle)
        {
            float newX = (float)(start.X * Math.Cos(angle) - start.Y * Math.Sin(angle));
            float newY = (float)(start.X * Math.Sin(angle) + start.Y * Math.Cos(angle));
            return new Vector3(newX,newY,1);
        }

        public static float LenFor2Dimension(this Vector3 v)
        {
            var copy = new Vector2(v.X, v.Y);
            return copy.Length();
        }
        public static double Angle(this Vector3 first) => Math.Atan2(first.Y, first.X);
    }
}
