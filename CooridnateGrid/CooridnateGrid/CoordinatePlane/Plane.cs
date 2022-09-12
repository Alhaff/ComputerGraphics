using CooridnateGrid.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CooridnateGrid.CoordinatePlane
{
    public abstract class Plane
    {
        public List<DrawnObject> Objects { get; }

        public  WriteableBitmap WrBitmap { get; set; }
        public readonly int StepInPixels;
        public Plane(int bitmapWidth, int bitmapHeight, int stepInPixels)
        {
            Objects = new List<DrawnObject>();
            WrBitmap = BitmapFactory.New(bitmapWidth, bitmapHeight);
            StepInPixels = stepInPixels;
        }
        public virtual void AddObject(DrawnObject obj)
        {
            Objects.Add(obj);
        }

        public virtual void RemoveObject(DrawnObject obj)
        {
            Objects.Remove(obj);
        }
        internal void DrawObj(DrawnObject obj)
        {
            var lineBuilder = new LinkedList<Vector2>();
            foreach (var contour in obj.GetContourPoints())
            {
                foreach (var points in contour.Select(point => ToBitmapCoord(obj.TransformFunctions(point)))
                                              .LineCreator())
                {
                        WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                            (int)points.Item2.X, (int)points.Item2.Y, obj.MyColor);
                }
            }
        }
        public virtual void Draw()
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
        internal static System.Drawing.Imaging.PixelFormat ConvertPixelFormat(System.Windows.Media.PixelFormat sourceFormat)
        {
            if( PixelFormats.Bgr24 == sourceFormat)
                    return System.Drawing.Imaging.PixelFormat.Format24bppRgb;    
            if (PixelFormats.Bgra32 == sourceFormat)
                    return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            if (PixelFormats.Bgr32 == sourceFormat)
                    return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
            if (PixelFormats.Pbgra32 == sourceFormat)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            return new System.Drawing.Imaging.PixelFormat();
        }
        public virtual Vector2 ToBitmapCoord(Vector2 planeCoord)
        {
            throw new NotImplementedException();
        }
    }
}
