using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    public static class Vector2Extensions
    {
        public static Vector2 Rotate(this Vector2 start, double angle)
        {
            float newX = (float)(start.X * Math.Cos(angle) - start.Y * Math.Sin(angle));
            float newY = (float)(start.X * Math.Sin(angle) + start.Y * Math.Cos(angle));
            return new Vector2(newX,newY);
        }

        public static double Angle(this Vector2 first) => Math.Atan2(first.Y, first.X);
    }
}
