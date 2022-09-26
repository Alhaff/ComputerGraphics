using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CooridnateGrid.ExtensionsClasses;
using CooridnateGrid.DrawnObjects;

namespace CooridnateGrid.Transformation
{
    public class RotateTransformation : Transformation
    {
        #region Variables
        private PointOnPlane  _center = new PointOnPlane(new Vector3(0,0,0));
        private double _angle = 0;
        #endregion

        #region Propreties
        public PointOnPlane CenterPoint { get => _center; }
        public Vector3 Center
        {
            get { return _center.Center; }
            set { 
                _center.Center = value;
                OnPropertyChanged("Center");
            }
        }
        public double Angle
        {
            get { return (Math.Round(_angle / (Math.PI /180))); }
            set { 
                _angle = value * (Math.PI/180);
                OnPropertyChanged("Angle");
            }
        }

        private float CosAngle { get=> (float)Math.Cos(_angle); }
        private float SinAngle { get => (float)Math.Sin(_angle); }

        private MyMatrix3x3 RotationMatrix
        {
            get =>
                new MyMatrix3x3
                (
                     CosAngle, SinAngle, 0,
                    -1 * SinAngle, CosAngle, 0,
                    (-1 * Center.X * (CosAngle - 1) + Center.Y * SinAngle), 
                    (-1 * Center.X * SinAngle - Center.Y * (CosAngle - 1)), 1
                );
        }
        #endregion
        public override Func<Vector3, Vector3> Transform => v =>  v * RotationMatrix;
        // Vector Form - ((v  - Center).Rotate(_angle) + Center)
    }
}
