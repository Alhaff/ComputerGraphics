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

namespace CooridnateGrid.DrawnObjects
{
    public class Circle : DrawnObject
    {
        #region Variables

        private float _r;
        private Vector3 _center;
        private Vector3 _startBreakPoint;
        private Vector3 _endBreakPoint;

        #endregion

        #region Propreties
  
        public Vector3 StartBreakPoint
        {
            get { return _startBreakPoint; }
            set {
                    if (value.LenFor2Dimension() <= R + 1E-5)
                    {
                    _startBreakPoint = value;
                    OnPropertyChanged("StartBreakPoint");
                    }
                }
        }
       
        public Vector3  EndBreakPoint
        {
            get { return _endBreakPoint; }
            set {
               
                if ( value.LenFor2Dimension() <= R + 1E-5)
                {
                    _endBreakPoint = value;
                    OnPropertyChanged("EndBreakPoint");
                }
            }
        }

        public Vector3 Center
        {
            get { return _center; }
            set { _center = value;
                OnPropertyChanged("Center");
            }
        }

        public float R
        {
            get { return _r; }
            set {
                _r = value;
                MoveBreakPoint(Center,value);
                OnPropertyChanged("R"); }
        }

        #endregion


        public Circle()
        {
            Center = new Vector3(0,0,1);
            R = 0;
            MyColor = Color.FromRgb(0, 255, 0);
        }

        public Circle(Vector3 startCoord, float r, Color color)
        {
            Center = startCoord;
            R = r;
            MyColor = color;
            StartBreakPoint =( Center + new Vector3(R,0,0));
            EndBreakPoint = (Center + new Vector3(R, 0, 0));
        }

        public Circle(Vector3 startCoord, float r, Vector3 startBreakPoint, Vector3 endBreakPoint, Color color)
        {
            Center = startCoord;
            R = r;
            StartBreakPoint = startBreakPoint;
            EndBreakPoint = endBreakPoint;
            MyColor = color;
        }
        private void MoveBreakPoint(Vector3 newCenter, float newR)
        {
            var startAngle = StartBreakPoint.Angle();
            var endAngle = EndBreakPoint.Angle();
            StartBreakPoint = Line.GetLineEndPoint(newCenter, newR, startAngle);
            EndBreakPoint = Line.GetLineEndPoint(newCenter, newR, endAngle);
        }
        internal IEnumerable<Vector3> GetCirclePoints()
        {
            var startAngle = StartBreakPoint.Angle() < 0? 2 * Math.PI - Math.Abs(StartBreakPoint.Angle()) : StartBreakPoint.Angle();
            var endAngle = EndBreakPoint.Angle() <= 0 ? 2 * Math.PI - Math.Abs(EndBreakPoint.Angle()) : EndBreakPoint.Angle();
            var start = -2* Math.PI + Math.Max(endAngle, startAngle);
            var end = Math.Min(endAngle, startAngle);
            end = end == 0 ? 2 * Math.PI : end;
            for (double t = start; t <= end; t += Math.PI / 180)
            {
                yield return Line.GetLineEndPoint(Center, R, t);
            }
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            yield return GetCirclePoints();
        }
    }
}
