using CooridnateGrid.DrawnObjects;
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
using CooridnateGrid.ExtensionsClasses;
using System.Drawing;

namespace CooridnateGrid.CoordinatePlane
{
    public class MyPlane : INotifyPropertyChanged
    {
        #region Variables
        private int _stepInPixels = 20;
        private double _skaleCoef = 1;
        #endregion

        #region Propreties
        public Func<Vector3, Vector3> Transform { get; set; } = v => v;
        public List<DrawnObject> Objects { get; }
        public int BitmapWidth { get; set; }
        public int BitmapHeight { get; set; }
        public int StepInPixels
        {

            get => _stepInPixels;

            set
                {
                    if (value >= 10 && value <= 80)
                    {
                        _stepInPixels = value;
                        OnPropertyChanged("StepInPixels");
                    }
                }
            
        }

        public float Dx { get; set; } = 0;
        public float Dy { get; set; } = 0;

        public double ScaleCoef
        {
            get => _skaleCoef;
            set
                {
                    if(value > 0) _skaleCoef = value;
                }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Constructors
        public MyPlane(int bitmapWidth, int bitmapHeight, int stepInPixels)
        {
            Objects = new List<DrawnObject>();
            BitmapWidth = bitmapWidth;
            BitmapHeight = bitmapHeight;
            StepInPixels = stepInPixels;
        }
        #endregion

        #region Methods
        internal static System.Drawing.Imaging.PixelFormat ConvertPixelFormat(System.Windows.Media.PixelFormat sourceFormat)
        {
            if (PixelFormats.Bgr24 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            if (PixelFormats.Bgra32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            if (PixelFormats.Bgr32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
            if (PixelFormats.Pbgra32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            return new System.Drawing.Imaging.PixelFormat();
        }

        internal void DrawObj(DrawnObject obj, Graphics g)
        {
            var lineBuilder = new LinkedList<Vector3>();
            foreach (var contour in obj.GetContourPoints())
            {
                Func<Vector3,Vector3> ToBitmap = obj.IsPlaneTransfromMe ? ToBitmapCoord : ToBitmapCoordWithoutTransform;
                foreach (var points in contour.Select(point => ToBitmap(point))
                                              .LineCreator())
                {
                     DrawLine(g, points, obj.MyColor);
                }
                
            }
        }
        public static bool IsNormalValue(Vector3 point)
        {
            return float.IsFinite(point.X) && float.IsFinite(point.Y);
                
        }
        internal void DrawLine(Graphics g, Tuple<Vector3,Vector3> points, System.Windows.Media.Color Color)
        {
            System.Drawing.Color color =
                        System.Drawing.Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
            System.Drawing.Pen pen = new System.Drawing.Pen(new SolidBrush(color));
            var vectStart = points.Item1;
            var vectEnd = points.Item2;
            if (IsNormalValue(vectStart) && IsNormalValue(vectEnd))
            {
                Point start = new Point((int)vectStart.X, (int)vectStart.Y);
                Point end = new Point((int)vectEnd.X, (int)vectEnd.Y);
                g.DrawLine(pen, start, end);
            }
        }
        public void AddObject(DrawnObject obj)
        {
            Objects.Add(obj);
        }

        public void AddObject(params DrawnObject[] objects)
        {
            foreach(var obj in objects)
            {
                AddObject(obj);
            }
        }

        public void RemoveObject(DrawnObject obj)
        {
            Objects.Remove(obj);
        }

        public void RemoveObject(params DrawnObject[] objects)
        {
            foreach(var obj in objects)
            {
                RemoveObject(obj);
            }
        }
     
        public void Draw(Graphics g)
        {
                g.Clear(System.Drawing.Color.White);
                foreach (var obj in Objects)
                {
                    if (obj != null)
                    {
                        if (obj is IDrawingSelf)
                        {
                            ((IDrawingSelf)obj).Draw(this, g);
                        }
                        else
                        {
                            DrawObj(obj, g);
                        }
                    }
                }
            
        }

        public Vector3 ToBitmapCoord(Vector3 planeCoord)
        {
            var transformed = Transform(planeCoord);
            var x = (transformed.X * StepInPixels)/ ScaleCoef + BitmapWidth / 2 + Dx;
            var y = (-(transformed.Y * StepInPixels)/ ScaleCoef + BitmapHeight / 2) + Dy;
            return new Vector3((float)x, (float)y, 1);
        }

        public Vector3 ToBitmapCoordWithoutTransform(Vector3 planeCoord)
        {
            var x = (planeCoord.X * StepInPixels) / ScaleCoef + BitmapWidth / 2 + Dx;
            var y = -(planeCoord.Y * StepInPixels) / ScaleCoef + BitmapHeight / 2 + Dy;
            return new Vector3((float)x, (float)y, 1);
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
