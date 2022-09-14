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
                _internalCircle = value;
                OnPropertyChanged("InternalCircle");
            }
        }

        public Circle ExternalCircle
        {
            get { return _externalCircle; }
            private set
            {
                _externalCircle = value;
                OnPropertyChanged("ExternalSector");
            }
        }

        public float InternalHeight
        {
            get { return _internalHeight; }
            set
            {
                _internalHeight = value;
                OnPropertyChanged("InternalHeight");
            }
        }

        public float InternalWidth
        {
            get { return _internalWidth; }
            set { 
                _internalWidth = value;
                if (InternalCircle != null)
                {
                    InternalCircle.StartBreakPoint =
                        GetBreakPoint(InternalCircle.R, -Math.PI / 2 - Angle(value, InternalCircle.R));
                    InternalCircle.EndBreakPoint =
                       GetBreakPoint(InternalCircle.R, -Math.PI / 2 + Angle(value, InternalCircle.R));
                }
                OnPropertyChanged("InternalWidth");
            }
        }

        public float ExternalHeigth
        {
            get { return _externalHeight; }
            set
            {
                _externalHeight = value;
                OnPropertyChanged("ExternalHeight");
            }
        }

        public float ExternalWidth
        {
            get { return _externalWidth; }
            set
            {
                _externalWidth = value;
                if (ExternalCircle != null)
                {
                    ExternalCircle.StartBreakPoint =
                        GetBreakPoint(ExternalCircle.R, -Math.PI / 2 - Angle(value, ExternalCircle.R));
                    ExternalCircle.EndBreakPoint =
                       GetBreakPoint(ExternalCircle.R, -Math.PI / 2 + Angle(value, ExternalCircle.R));
                }
                OnPropertyChanged("ExternalWidth");
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
                _bottomRectangleHeight = value;
                OnPropertyChanged("BottomRectangleHeight");
            }
        }
        #endregion

        private double Angle(float lenBeetweenTwoBreakPoint, float circleR)
        {
            return Math.Asin((lenBeetweenTwoBreakPoint/2) / circleR);
        }

        private Vector3 GetBreakPoint(float circleR, double angle)
        {
            var RVect = new Vector3(circleR, 0, 1);
            return RVect.Rotate(angle);
        }

        public Lab1Drawing()
        {
            MyColor = Color.FromRgb(0, 0, 0);
            InternalHeight = 1.75f;
            InternalWidth = 0.5f;
            ExternalHeigth = 2.15f;
            ExternalWidth = 1.1f;
            var inR = 0.75f;
            var extR = 1.375f;
            BottomRectangleWidth = 2.4f;
            BottomRectangleHeight = 0.2f;
            var center = new Vector3(0, 0,1);
            var RVect = new Vector3(0.75f, 0, 1);
            var inAngle = Angle(InternalWidth,inR);
            var extAngle = Angle(ExternalWidth,extR);
            var startInBreakPoint =  GetBreakPoint(inR, -Math.PI/2  - inAngle);
            var endInBreakPoint = GetBreakPoint(inR, -Math.PI / 2 + inAngle);
            var startExtBreakPoint = GetBreakPoint(extR, -Math.PI / 2 - extAngle);
            var endExtBreakPoint = GetBreakPoint(extR, -Math.PI / 2 + extAngle);
            InternalCircle = new Circle(center, inR,startInBreakPoint,endInBreakPoint, MyColor);
            ExternalCircle = new (center, extR,startExtBreakPoint,endExtBreakPoint, MyColor);
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
            var extHeight = Line.GetLineEndPoint(center, ExternalHeigth, -Math.PI / 2);
            var leftUpMiddlePoint = new Vector3(start.X, extHeight.Y, 1);
            var rightUpMidlePoint = new Vector3(end.X, extHeight.Y, 1);
            var len = (BottomRectangleWidth - (end - start).Length()) / 2f;
            var leftUpCornerPoint = Line.GetLineEndPoint(leftUpMiddlePoint, len, Math.PI);
            var leftDownCornerPoint = Line.GetLineEndPoint(leftUpCornerPoint, BottomRectangleHeight, - Math.PI/2);
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

        public override IEnumerable<IEnumerable<Vector3>> GetContourPoints()
        {
            yield return InternalCircle.GetCirclePoints();
            yield return GetInternalRectangle();
            yield return ExternalCircle.GetCirclePoints();
            yield return GetExternalRectangle();
        }

    }
}
