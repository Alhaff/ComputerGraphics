using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CooridnateGrid.ExtensionsClasses;

namespace CooridnateGrid.Transformation
{
    public class RotateTransformation : Transformation
    {
        #region Variables
        private Vector3 _center = new Vector3(0,0,0);
        private double _angle = 0;
        #endregion

        #region Propreties
        public Vector3 Center
        {
            get { return _center; }
            set { 
                _center = value;
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
                    (-1 * _center.X * (CosAngle - 1) + _center.Y * SinAngle), 
                    (-1 *_center.X * SinAngle - _center.Y * (CosAngle - 1)), 1
                );
                
        }
        #endregion
        public override Func<Vector3, Vector3> Transform => v =>  v * RotationMatrix;
        // Vector Form - ((v  - Center).Rotate(_angle) + Center)
    }
}
