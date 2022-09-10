using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CooridnateGrid.CoordinatePlane;

namespace CooridnateGrid.DrawingObjects
{
    public class CoordinateAxis : IDrawingObject
    {
        private int width;
        public int Width {
            get { return width; }
            set { IChanged = true; width = value; }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set { IChanged = true; height = value; }
        }
        private Func<Vector2, Vector2> transformFunctions;
        public Func<Vector2, Vector2> TransformFunctions 
        {
            get => transformFunctions;
            set { IChanged = true; transformFunctions = value; } 
        }
        public bool IChanged { get; set; }

        public CoordinateAxis(int width, int heigth)
        {
           Width = width;
           Height = heigth;
           TransformFunctions = v => v;
        }
        private Vector2 Point(int x, int y) => new Vector2(- Width / 2 + x, - Height / 2 + y);

        public void Draw(CoordinatePlane.Plane plane)
        {
            var wr = plane.WrBitmap;
            for(var i = 0; i < Width; i++)
            {
                var prev = plane.ToBitmapCoord(TransformFunctions(Point(i,0)));
                var curr = plane.ToBitmapCoord(TransformFunctions(Point(i, Height)));
                    wr.DrawLine((int)prev.X, (int)prev.Y,
                           (int)curr.X, (int)curr.Y, Color.FromRgb(0, 0, 0));
            }

            for (var i = 0; i < Height; i++)
            {
                var prev = plane.ToBitmapCoord(TransformFunctions(Point(0, i)));
                var curr = plane.ToBitmapCoord(TransformFunctions(Point(Width, i)));
                wr.DrawLine((int)prev.X, (int)prev.Y,
                       (int)curr.X, (int)curr.Y, Color.FromRgb(0, 0, 0));
            }

        }
    }
}
