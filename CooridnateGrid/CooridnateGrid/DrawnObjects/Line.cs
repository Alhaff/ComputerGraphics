using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class Line : DrawnObject
    {
        private Vector2 _start;
        private double _length;
        private double _angle;

        public Vector2 Start
        {
            get { return _start; }
            set { 
                    _start = value;
                    OnPropertyChanged("Start");
                }
        }

        public double Length
        {
            get { return _length; }
            set { 
                    _length = value;
                    OnPropertyChanged("Length");
                }
        }

        public double Angle
        {
            get { return _angle; }
            set { _angle = value;
                    OnPropertyChanged("Length");
                }
        }

        public Vector2 End { get => (Start + new Vector2((float)Length, 0)).Rotate(Angle); }

        public Line(Vector2 start, double length, double angle, Color color)
        {
            Start = start;
            Length = length;
            Angle = angle;
        }

        private IEnumerable<Vector2> GetLinePoint()
        {
            yield return Start;
            yield return End;
        }
        public override IEnumerable<IEnumerable<Vector2>> GetContourPoints()
        {
            yield return GetLinePoint();
        }
    }
}
