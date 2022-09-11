using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CooridnateGrid.CoordinatePlane;

namespace CooridnateGrid.DrawnObjects
{
    public class CoordinateAxis : IDrawnObject
    {
        private int width;
        public int Width {
            get { return width; }
            set { width = value;
                OnPropertyChanged("Width");
            }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set { height = value;
                OnPropertyChanged("Height");
            }
        }
        private Func<Vector2, Vector2> transformFunctions;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public Func<Vector2, Vector2> TransformFunctions 
        {
            get => transformFunctions;
            set { transformFunctions = value;
                OnPropertyChanged("TransformFunctions");
            } 
        }
        public CoordinateAxis()
        {
            Width = 0;
            Height = 0;
            TransformFunctions = v => v;
        }
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
