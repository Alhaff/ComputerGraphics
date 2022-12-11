using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CooridnateGrid.ExtensionsClasses;

namespace CooridnateGrid.DrawnObjects
{
    public class Epicycloid : DrawnObject
    {
        private float _r;
        private float _R;
        private Vector3 _center;
        private double _square;
        private double _maxAngle = 2 * Math.PI;
         public Vector3 Center
        {
            get { return _center; }
            set { _center = value;
                OnPropertyChanged("Center");
            }
        }

        public float r
        {
            get { return (float)Math.Round(_r,2); }
            set {
                _r = value;
                OnPropertyChanged("r");
                OnPropertyChanged("k");
                OnPropertyChanged("Square");
                OnPropertyChanged("Length");
            }
        }

        public float R
        {
            get { return (float)Math.Round(_R, 2); }
            set
            {
                _R = value;
                OnPropertyChanged("R");
                OnPropertyChanged("k");
                OnPropertyChanged("Square");
                OnPropertyChanged("Length");
            }
        }

        public double k { 
            get => Math.Round(R / r,2);
            set => r = (float)(R / value);
        }

        private double _scaleCoef;

        public double Square { 
            get => Math.Round(Math.PI * r * r *(k +1) 
                *(k +2), 4); 
            
        }
        public double Length
        {
            get => Math.Round(8 * r * ScaleCoef * (1 + k), 4);

        }

        public double ScaleCoef
        {
            get { return _scaleCoef; }
            set {
                if (value >= Math.Abs(_r / _R))
                {
                    _scaleCoef = value;
                    OnPropertyChanged("ScaleCoef");
                    OnPropertyChanged("Square");
                    OnPropertyChanged("Length");
                }
            }
        }



        public Epicycloid()
        {
            r = 3;
            R = 6;
            ScaleCoef = 1;
            Center = new Vector3(0, 0, 1);
            MyColor = Color.FromRgb(0, 0, 0);
        }
        public Epicycloid(float r, float R)
        {
            _r = r;
            _R = R; 
        }
        public Vector3 GetEcicycloidPoint(double angle)
        {
            var x = (r * ScaleCoef) * (k + 1) * (Math.Cos(angle) - (Math.Cos((k + 1) * angle) / (k + 1)));
            var y = (r * ScaleCoef) * (k + 1) * (Math.Sin(angle) - (Math.Sin((k + 1) * angle) / (k + 1)));
            return new Vector3((float)x, (float)y,1);
        }
        private IEnumerable<Vector3> GetEpicycloidContourPoints()
        {
            var startPoint = GetEcicycloidPoint(0);
            yield return startPoint;
            for (double t = Math.PI / 360; t <= 20 * Math.PI; t += Math.PI / 360)
            {
                var currPoint = GetEcicycloidPoint(t);
                yield return currPoint;
                if (currPoint.IsSamePoint(startPoint, 1E-5))
                {
                    yield break;
                }
            }
        }

        public Tuple<bool, Vector3, double> IsPointInEpicycloidVicinity(Vector3 point, double precision = 1E-3)
        {
            var startPoint = GetEcicycloidPoint(0);
            for (double t = Math.PI / 360; ; t += Math.PI / 360)
            {
                var currPoint = GetEcicycloidPoint(t);
                if (currPoint.IsSamePoint(point, precision))
                {
                    return Tuple.Create(true, currPoint, t);
                }
                if(currPoint.IsSamePoint(startPoint, 1E-7))
                {
                    return Tuple.Create(false, point, double.NaN);
                }
            }
        }
        
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            yield return GetEpicycloidContourPoints();
        }
        public void ReturnToDefaultSize()
        {
            R = 6;
            r = 3;
            ScaleCoef = 1;
            Center = new Vector3(0, 0, 1);
        }
    }
}
