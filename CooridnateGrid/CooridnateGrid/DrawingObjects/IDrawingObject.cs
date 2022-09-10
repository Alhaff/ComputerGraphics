using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CooridnateGrid.DrawingObjects
{
    public interface IDrawingObject
    {
        public void Draw(CoordinatePlane.Plane plane);

        public Func<Vector2, Vector2> TransformFunctions { get; set; }
        public bool IChanged { get; set; }
    }
}
