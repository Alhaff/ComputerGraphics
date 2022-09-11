using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CooridnateGrid.DrawnObjects
{
    public interface IDrawnObject : INotifyPropertyChanged
    {
        public IEnumerable<IEnumerable<Vector2>> GetContourPoints();
        public Color MyColor { get; set; }
        public Func<Vector2, Vector2> TransformFunctions { get; set; }
    }
}
