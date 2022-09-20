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

namespace CooridnateGrid.CoordinatePlane
{
    public class MyPlane : INotifyPropertyChanged
    {
        #region Variables
        private int _stepInPixels = 20;
        #endregion

        #region Propreties
        public Func<Vector3, Vector3> Transform { get; set; } = v => v;
        public List<DrawnObject> Objects { get; }
        public  WriteableBitmap WrBitmap { get; set; }
       
        public int StepInPixels
        {

            get => _stepInPixels;

            set
                {
                _stepInPixels = value;
                OnPropertyChanged("StepInPixels");
                }
            
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Constructors
        public MyPlane(int bitmapWidth, int bitmapHeight, int stepInPixels)
        {
            Objects = new List<DrawnObject>();
            WrBitmap = BitmapFactory.New(bitmapWidth, bitmapHeight);
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

        internal void DrawObj(DrawnObject obj)
        {
            var lineBuilder = new LinkedList<Vector3>();
            foreach (var contour in obj.GetContourPoints())
            {
                foreach (var points in contour.Select(point => ToBitmapCoord(point))
                                              .LineCreator())
                {
                    WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                        (int)points.Item2.X, (int)points.Item2.Y, obj.MyColor);
                }
            }
        }

        public void AddObject(DrawnObject obj)
        {
            Objects.Add(obj);
        }

        public void RemoveObject(DrawnObject obj)
        {
            Objects.Remove(obj);
        }
     
        public void Draw()
        {
            WrBitmap.Clear();
            foreach (var obj in Objects)
            {
                if (obj != null)
                {
                    if (obj is IDrawingSelf)
                    {
                        ((IDrawingSelf)obj).Draw(this);
                    }
                    else
                    {
                        DrawObj(obj);
                    }
                }
          }
        }

        public Vector3 ToBitmapCoord(Vector3 planeCoord)
        {
            var transformed = Transform(planeCoord);
            return new Vector3((float)(transformed.X * StepInPixels + WrBitmap.Width / 2),
                               (float)(-(transformed.Y * StepInPixels) + WrBitmap.Height / 2),
                               1);
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
