using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CooridnateGrid.DrawnObjects;

namespace CooridnateGrid.CoordinatePlane
{
    public class CartesianPlane : Plane
    {
        public CartesianPlane(int bitmapWidth, int bitmapHeight, int step) 
            : base(bitmapWidth, bitmapHeight, step)
        { }

        public override Vector2 ToBitmapCoord(System.Numerics.Vector2 planeCoord)
        {
            return new Vector2((int)((planeCoord.X * StepInPixels) + WrBitmap.Width / 2), 
                               (int)(-(planeCoord.Y * StepInPixels) + WrBitmap.Height / 2));
        }
    }
}
