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

namespace CooridnateGrid.CoordinatePlane
{
    public class MyPlane
    {
        public Func<Vector3, Vector3> Transform { get; set; } = v => v;
        public List<DrawnObject> Objects { get; }
        public  WriteableBitmap WrBitmap { get; set; }
        public readonly int StepInPixels;
        public MyPlane(int bitmapWidth, int bitmapHeight, int stepInPixels)
        {
            Objects = new List<DrawnObject>();
            WrBitmap = BitmapFactory.New(bitmapWidth, bitmapHeight);
            StepInPixels = stepInPixels;
        }
        public void AddObject(DrawnObject obj)
        {
            Objects.Add(obj);
        }

        public void RemoveObject(DrawnObject obj)
        {
            Objects.Remove(obj);
        }
        internal void DrawObj(DrawnObject obj)
        {
            var lineBuilder = new LinkedList<Vector3>();
            foreach (var contour in obj.GetContourPoints())
            {
                foreach (var points in contour.Select(point => ToBitmapCoord(obj.TransformMe(point)))
                                              .LineCreator())
                {
                        WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                            (int)points.Item2.X, (int)points.Item2.Y, obj.MyColor);
                }
            }
        }
        public void Draw()
        {
                WrBitmap.Clear();
                foreach (var obj in Objects)
                {
                    if(obj is IDrawingSelf)
                    {
                         ((IDrawingSelf)obj).Draw(this);
                    }
                    else {
                        DrawObj(obj);
                    }
                }
        }
       
        public virtual Vector3 ToBitmapCoord(Vector3 planeCoord)
        {
            var transformed = Transform(planeCoord);
            return new Vector3((float)(transformed.X * StepInPixels + WrBitmap.Width / 2),
                               (float)(-(transformed.Y * StepInPixels) + WrBitmap.Height / 2),
                               1);
        }

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

    }
}
