using CooridnateGrid.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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
        private void DrawObj(DrawnObject obj)
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
                    DrawObj(obj);
                }
        }

        public virtual Vector2 ToBitmapCoord(Vector2 planeCoord)
        {
            throw new NotImplementedException();
        }
    }
}
