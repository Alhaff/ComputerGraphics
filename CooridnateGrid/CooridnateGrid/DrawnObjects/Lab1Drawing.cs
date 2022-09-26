using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class Lab1Drawing : DrawnObject
    {
        #region variables
        private Circle _internalCircle;
        private Circle _externalCircle;
        private float _internalHeight;
        private float _internalWidth;
        private float _externalHeight;
        private float _externalWidth;
        private float _bottomRectangleWidth;
        private float _bottomRectangleHeight;
        #endregion

        #region Properties

        public Circle InternalCircle
        {
            get { return _internalCircle; }
            private set
            {
                if (value.R < _externalCircle.R)
                {
                    _internalCircle = value;
                    OnPropertyChanged("InternalCircle");
                }
            }
        }

        public Circle ExternalCircle
        {
            get { return _externalCircle; }
            private set
            {
                if (value.R > _internalCircle.R)
                {
                    _externalCircle = value;
                    OnPropertyChanged("ExternalSector");
                }
            }
        }

        public float InternalHeight
        {
            get { return _internalHeight; }
            set
            {
                if (value > 0 && value < ExternalHeight)
                {
                    _internalHeight = value;
                    OnPropertyChanged("InternalHeight");
                }
            }
        }

        public float InternalWidth
        {
            get { return _internalWidth; }
            set {
                if (value > 0 && value < ExternalWidth)
                {
                    _internalWidth = value;
                    if (InternalCircle != null)
                    {
                        InternalCircle.StartBreakPoint =
                            GetBreakPoint(InternalCircle, -Math.PI / 2 - Angle(value, InternalCircle.R));
                        InternalCircle.EndBreakPoint =
                           GetBreakPoint(InternalCircle, -Math.PI / 2 + Angle(value, InternalCircle.R));
                    }
                    OnPropertyChanged("InternalWidth");
                }
            }
        }

        public float ExternalHeight
        {
            get { return _externalHeight; }
            set
            {
                if (value > 0 && value > InternalHeight)
                {
                    _externalHeight = value;
                    OnPropertyChanged("ExternalHeight");
                }
            }
        }

        public float ExternalWidth
        {
            get { return _externalWidth; }
            set
            {
                if (value > 0 && value > InternalWidth)
                {
                    _externalWidth = value;
                    if (ExternalCircle != null)
                    {
                        ExternalCircle.StartBreakPoint =
                            GetBreakPoint(ExternalCircle, -Math.PI / 2 - Angle(value, ExternalCircle.R));
                        ExternalCircle.EndBreakPoint =
                           GetBreakPoint(ExternalCircle, -Math.PI / 2 + Angle(value, ExternalCircle.R));
                    }
                    OnPropertyChanged("ExternalWidth");
                }
            }
        }

        public float BottomRectangleWidth
        {
            get { return _bottomRectangleWidth; }
            set
            {
                _bottomRectangleWidth = value;
                OnPropertyChanged("BottomRectangleWidth");
            }
        }

        public float BottomRectangleHeight
        {
            get { return _bottomRectangleHeight; }
            set
            {
                if (value > 0)
                {
                    _bottomRectangleHeight = value;
                    OnPropertyChanged("BottomRectangleHeight");
                }
            }
        }

        public float InR 
        {
            get => InternalCircle.R;
            set
            {
                if (value < ExternalCircle.R)
                {
                    InternalCircle.R = value;
                    OnPropertyChanged("InR");
                }
            }
        }

        public float ExtR 
        {
            get => ExternalCircle.R;
            set
            {
                if(value > InternalCircle.R)
                {
                    ExternalCircle.R = value;
                    OnPropertyChanged("ExtR");
                }
            }
        }
        #endregion

        #region Constructors
        public Lab1Drawing()
        {
            MyColor = Color.FromRgb(0, 0, 0);
            _externalHeight = 2.15f * 4;
            _externalWidth = 1.1f * 4;
            _internalHeight = 1.75f * 4;
            _internalWidth = 0.5f * 4;
            var inR = 0.75f * 4;
            var extR = 1.375f * 4;
            _bottomRectangleWidth = 2.4f * 4;
            _bottomRectangleHeight = 0.2f * 4;
            var center = new Vector3(0, 0, 1);
            var inAngle = Angle(InternalWidth, inR);
            var extAngle = Angle(ExternalWidth, extR);
            _internalCircle = new Circle(center, inR, MyColor);
            _externalCircle = new Circle(center, extR, MyColor);
            var startInBreakPoint = GetBreakPoint(InternalCircle, -Math.PI / 2 - inAngle);
            var endInBreakPoint = GetBreakPoint(InternalCircle, -Math.PI / 2 + inAngle);
            var startExtBreakPoint = GetBreakPoint(ExternalCircle, -Math.PI / 2 - extAngle);
            var endExtBreakPoint = GetBreakPoint(ExternalCircle, -Math.PI / 2 + extAngle);
            InternalCircle.StartBreakPoint = startInBreakPoint;
            InternalCircle.EndBreakPoint = endInBreakPoint;
            ExternalCircle.StartBreakPoint = startExtBreakPoint;
            ExternalCircle.EndBreakPoint = endExtBreakPoint;
        }
        #endregion

        #region Methods
        private double Angle(float lenBeetweenTwoBreakPoint, float circleR)
        {
            return Math.Asin((lenBeetweenTwoBreakPoint/2) / circleR);
        }

        private Vector3 GetBreakPoint(Circle c,  double angle)
        {
            return Line.GetLineEndPoint(c.Center,c.R,angle);
        }
        
        public void ReturnToDefaultSize()
        {
            MyColor = Color.FromRgb(0, 0, 0);
            _externalHeight = 2.15f * 4;
            _externalWidth = 1.1f * 4;
            _internalHeight = 1.75f * 4;
            _internalWidth = 0.5f * 4;
            var inR = 0.75f * 4;
            var extR = 1.375f * 4;
            _bottomRectangleWidth = 2.4f * 4;
            _bottomRectangleHeight = 0.2f * 4;
            var center = new Vector3(0, 0, 1);
            var inAngle = Angle(InternalWidth, inR);
            var extAngle = Angle(ExternalWidth, extR);
            _internalCircle = new Circle(center, inR, MyColor);
            _externalCircle = new Circle(center, extR, MyColor);
            var startInBreakPoint = GetBreakPoint(InternalCircle, -Math.PI / 2 - inAngle);
            var endInBreakPoint = GetBreakPoint(InternalCircle, -Math.PI / 2 + inAngle);
            var startExtBreakPoint = GetBreakPoint(ExternalCircle, -Math.PI / 2 - extAngle);
            var endExtBreakPoint = GetBreakPoint(ExternalCircle, -Math.PI / 2 + extAngle);
            InternalCircle.StartBreakPoint = startInBreakPoint;
            InternalCircle.EndBreakPoint = endInBreakPoint;
            ExternalCircle.StartBreakPoint = startExtBreakPoint;
            ExternalCircle.EndBreakPoint = endExtBreakPoint;
        }

        internal IEnumerable<Vector3> GetInternalRectangle()
        {
            var center = InternalCircle.Center;
            var start = InternalCircle.StartBreakPoint;
            var end = InternalCircle.EndBreakPoint;
            var inHeight = Line.GetLineEndPoint(center, InternalHeight, -Math.PI / 2);
            var leftCornerPoint = new Vector3(start.X, inHeight.Y,1);
            var rightCornerPoint = new Vector3(end.X, inHeight.Y,1);
            yield return start; 
            yield return leftCornerPoint;
            yield return rightCornerPoint;
            yield return end;
        }

        internal IEnumerable<Vector3> GetExternalRectangle()
        {
            var start = ExternalCircle.StartBreakPoint;
            var end = ExternalCircle.EndBreakPoint;
            var center = ExternalCircle.Center;
            var extHeight = Line.GetLineEndPoint(center, ExternalHeight, -Math.PI / 2);
            var len = (BottomRectangleWidth - ExternalWidth) / 2f;
            var leftDownCornerPoint = Line.GetLineEndPoint(extHeight, BottomRectangleWidth/2f, Math.PI);
            var leftUpCornerPoint = Line.GetLineEndPoint(leftDownCornerPoint, BottomRectangleHeight, Math.PI/2);
            var leftUpMiddlePoint = Line.GetLineEndPoint(leftUpCornerPoint, len);
            var rightUpMidlePoint = Line.GetLineEndPoint(leftUpMiddlePoint, ExternalWidth);
            var rightDownCornerPoint = Line.GetLineEndPoint(leftDownCornerPoint, BottomRectangleWidth);
            var rightUpCornerPoint = Line.GetLineEndPoint(leftUpCornerPoint, BottomRectangleWidth);
            yield return start;
            yield return leftUpMiddlePoint;
            yield return leftUpCornerPoint;
            yield return leftDownCornerPoint;
            yield return rightDownCornerPoint;
            yield return rightUpCornerPoint;
            yield return rightUpMidlePoint;
            yield return end;
        }

        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            yield return InternalCircle.GetCirclePoints();
            yield return GetInternalRectangle();
            yield return ExternalCircle.GetCirclePoints();
            yield return GetExternalRectangle();
        }
        #endregion
    }
}
