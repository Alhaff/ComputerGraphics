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

namespace CooridnateGrid.DrawingObjects
{
    public class Circle : IDrawingObject
    {
        private Vector2 center;

        public Vector2 Center
        {
            get { return center; }
            set { center = value;
                OnPropertyChanged("Center");
            }
        }

        private Vector2 r;

        public Vector2 R
        {
            get { return r; }
            set {
                r = value; 
                OnPropertyChanged("R"); }
        }
        
        public bool IChanged { get; set; }

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
            set { IChanged = true; transformFunctions = value; }
        }

        public Circle(Vector2 startCoord, Vector2 r)
        {
            Center = startCoord;
            R = r;
            TransformFunctions = v => v;
        }

        public void Draw(CoordinatePlane.Plane plane)
        {
            var wr = plane.WrBitmap;
            foreach (var p in CirclePoints().Select(p=> plane.ToBitmapCoord(TransformFunctions(p))).Bigrams())
            {
                wr.DrawLine((int)p.Item1.X, (int)p.Item1.Y, 
                            (int)p.Item2.X, (int)p.Item2.Y, Color.FromRgb(255, 0, 0));
            }
        }

        private IEnumerable<Vector2> CirclePoints()
        {
            var points = new List<Tuple<int, int>>();
            for (double t = 0; t <= 2 * Math.PI; t += Math.PI / 24)
            {
                var x = R.Length() * Math.Cos(t) + Center.X;
                var y = R.Length() * Math.Sin(t) + Center.Y;
                yield return new Vector2((float)x, (float)y);
            }
            yield return new Vector2((float)(R.Length() * Math.Cos(0) + Center.X),
                                     (float)(R.Length() * Math.Sin(0) + Center.Y));
        }
    }
}
