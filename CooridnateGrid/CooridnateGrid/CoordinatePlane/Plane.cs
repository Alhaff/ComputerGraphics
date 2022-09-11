using CooridnateGrid.DrawingObjects;
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
        public List<IDrawingObject> Objects { get; }

        public  WriteableBitmap WrBitmap { get; set; }
        public readonly int StepInPixels;
        public Plane(int bitmapWidth, int bitmapHeight, int stepInPixels)
        {
            Objects = new List<IDrawingObject>();
            WrBitmap = BitmapFactory.New(bitmapWidth, bitmapHeight);
            StepInPixels = stepInPixels;
        }
        public virtual void AddObject(IDrawingObject obj)
        {
            Objects.Add(obj);
        }

        public virtual void RemoveObject(IDrawingObject obj)
        {
            Objects.Remove(obj);
        }

        public virtual void Draw()
        {
                WrBitmap.Clear();
                foreach (var obj in Objects)
                {

                    obj.Draw(this);
                }
        }

        public virtual Vector2 ToBitmapCoord(Vector2 planeCoord)
        {
            throw new NotImplementedException();
        }
    }
}
