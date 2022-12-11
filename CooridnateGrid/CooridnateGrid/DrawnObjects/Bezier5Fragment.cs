using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class Bezier5Fragment : DrawnObject, IDrawingSelf
    {
        private PointOnPlane _APoint;
        private PointOnPlane _BPoint;
        private PointOnPlane _CPoint;
        private PointOnPlane _DPoint;
        private PointOnPlane _EPoint;
        private PointOnPlane _FPoint;

        public PointOnPlane APoint
        {
            get { return _APoint; }
            set { 

                if (_APoint == null)
                {
                    _APoint = value;
                    _APoint.PropertyChanged += (v, e) => OnPropertyChanged("A");
                }
                else
                {
                    _APoint = value;
                }
                OnPropertyChanged("APoint");
            }
        }

        public PointOnPlane BPoint
        {
            get { return _BPoint; }
            set
            {
                if (_BPoint == null)
                {
                    _BPoint = value;
                    _BPoint.PropertyChanged += (v, e) => OnPropertyChanged("B");
                }
                else
                {
                    _BPoint = value;
                }
                OnPropertyChanged("BPoint");
            }
        }
        public PointOnPlane CPoint
        {
            get { return _CPoint; }
            set
            {
                if (_CPoint == null)
                {
                    _CPoint = value;
                    _CPoint.PropertyChanged += (v, e) => OnPropertyChanged("C");
                }
                else
                {
                    _CPoint = value;
                }
                OnPropertyChanged("CPoint");
            }
        }
        public PointOnPlane DPoint
        {
            get { return _DPoint; }
            set
            {
                if (_DPoint == null)
                {
                    _DPoint = value;
                    _DPoint.PropertyChanged += (v, e) => OnPropertyChanged("D");
                }
                else
                {
                    _DPoint = value;
                }
                OnPropertyChanged("DPoint");
            }
        }
        public PointOnPlane EPoint
        {
            get { return _EPoint; }
            set
            {
                if (_EPoint == null)
                {
                    _EPoint = value;
                    _EPoint.PropertyChanged += (v, e) => OnPropertyChanged("E");
                }
                else
                {
                    _EPoint = value;
                }
                OnPropertyChanged("EPoint");
            }
        }
        public PointOnPlane FPoint
        {
            get { return _FPoint; }
            set
            {
                if (_FPoint == null)
                {
                    _FPoint = value;
                    _FPoint.PropertyChanged += (v, e) => OnPropertyChanged("F");
                }
                else
                {
                    _FPoint = value;
                }
                OnPropertyChanged("FPoint");
            }
        }

        public Vector3 A
        {
            get => APoint.Center;
            set
            {
                APoint.Center = value;
                OnPropertyChanged("A");
            }
        }
        public Vector3 B {
            get => BPoint.Center;
            set
            {
                BPoint.Center = value;
                OnPropertyChanged("B");
            }
        }
        public Vector3 C {
            get => CPoint.Center;
            set
            {
                CPoint.Center = value;
                OnPropertyChanged("C");
            }
        }
        public Vector3 D {
            get => DPoint.Center;
            set
            {
                DPoint.Center = value;
                OnPropertyChanged("D");
            }
        }
        public Vector3 E {
            get => EPoint.Center;
            set
            {
                EPoint.Center = value;
                OnPropertyChanged("E");
            }
        }
        public Vector3 F {
            get => FPoint.Center;
            set
            {
                FPoint.Center = value;
                OnPropertyChanged("F");
            }
        }

        public Bezier5Fragment(MyPlane pl)
        {
            APoint = new PointOnPlane(pl, new Vector3(6, 10.5f, 1));
            BPoint = new PointOnPlane(pl, new Vector3(6.7f, 11, 1));
            CPoint = new PointOnPlane(pl, new Vector3(10.5f, 11, 1));
            DPoint = new PointOnPlane(pl, new Vector3(10.5f, 5, 1));
            EPoint = new PointOnPlane(pl, new Vector3(7, 4, 1));
            FPoint = new PointOnPlane(pl, new Vector3(7.5f, 2, 1));
            APoint.TransformMe += this.TransformMe;
            BPoint.TransformMe += this.TransformMe;
            CPoint.TransformMe += this.TransformMe;
            DPoint.TransformMe += this.TransformMe;
            EPoint.TransformMe += this.TransformMe;
            FPoint.TransformMe += this.TransformMe;
            MyColor = Colors.Red;
        }
        private IEnumerable<Vector3> GetBezier5FragmentCountour()
        {
            for(double t = 0; t <=1; t+=0.025)
            {
                var res = (float)Math.Pow(1 - t, 5) * A +
                          5 * (float)Math.Pow(t, 1) * (float)Math.Pow(1 - t, 4) * B +
                          10 *(float)Math.Pow(t, 2) * (float)Math.Pow(1 - t, 3) * C +
                          10 * (float)Math.Pow(t, 3) * (float)Math.Pow(1 - t, 2) * D +
                          5 * (float)Math.Pow(t, 4) * (float)Math.Pow(1 - t, 1) * E +
                          (float)Math.Pow(t, 5) * F;
                yield return new Vector3(res.X, res.Y,1);
            }
            //yield return F;
        }
        private IEnumerable<Vector3> GetHexagonCountour()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
            yield return E;
            yield return F;
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            var tmpColor = MyColor;
            yield return GetBezier5FragmentCountour();
            MyColor = Colors.OrangeRed;
            yield return GetHexagonCountour();
            MyColor = tmpColor;
        }

        public void AddPointsOnCanvas(Canvas canvas)
        {
            APoint.AddPointOnCanvas(canvas);
            BPoint.AddPointOnCanvas(canvas);
            CPoint.AddPointOnCanvas(canvas);
            DPoint.AddPointOnCanvas(canvas);
            EPoint.AddPointOnCanvas(canvas);
            FPoint.AddPointOnCanvas(canvas);
        }

        public void RemovePointsFromCanvas(Canvas canvas)
        {
            APoint.RemovePointFromCanvas(canvas);
            BPoint.RemovePointFromCanvas(canvas);
            CPoint.RemovePointFromCanvas(canvas);
            DPoint.RemovePointFromCanvas(canvas);
            EPoint.RemovePointFromCanvas(canvas);
            FPoint.RemovePointFromCanvas(canvas);
        }

        public void Draw(MyPlane pl, Graphics g)
        {
            pl.DrawObj(this, g);
            APoint.Draw(pl, g);
            BPoint.Draw(pl, g);
            CPoint.Draw(pl, g);
            DPoint.Draw(pl,g);
            EPoint.Draw(pl, g);
            FPoint.Draw(pl, g);
        }
    }
}
