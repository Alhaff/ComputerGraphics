using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class Rotate3DTransformations : Transformation
    {
        #region Variables
        private double _angleX = 0;
        private double _angleY = 0;
        private double _angleZ = 0;
        private Vector3 _centerPoint = new Vector3(0,0,0);
        public Vector3 Center
        {
            get
            {
                return _centerPoint;
            }
            set
            {
                _centerPoint = value;
                OnPropertyChanged("Center");

            }
        }
        public double SetXAngLe
        {
           get => _angleX;
            set
                {
                _angleX = value;
                OnPropertyChanged("AngleX");
            }
        }
        public double SetYAngLe
        {
            get => _angleY;
            set
            {
                _angleY = value;
                OnPropertyChanged("AngleY");
            }
        }
        public double SetZAngLe
        {
            get => _angleZ;
            set
            {
                _angleZ = value;
                OnPropertyChanged("AngleZ");
            }
        }
        public double AngleX
        {
            get { return (Math.Round(_angleX / (Math.PI / 180))); }
            set
            {
                _angleX = value * (Math.PI / 180);
                OnPropertyChanged("AngleX");
            }
        }
        public double AngleY
        {
            get { return (Math.Round(_angleY / (Math.PI / 180))); }
            set
            {
                _angleY = value * (Math.PI / 180);
                OnPropertyChanged("AngleY");
            }
        }
        public double AngleZ
        {
            get { return (Math.Round(_angleZ / (Math.PI / 180))); }
            set
            {
                _angleZ = value * (Math.PI / 180);
                OnPropertyChanged("AngleZ");
            }
        }

        private float Cos(double angle) => (float)Math.Cos(angle);
        private float Sin(double angle) => (float)Math.Sin(angle);
        public override Func<Vector3, Vector3> Transform => p =>
        {
            var tmp = p * OffsetMatrix(-1 * Center);
          
            tmp = tmp * RotateX;
            tmp = tmp * RotateY;
            tmp = tmp * RotateZ;
            tmp = tmp * OffsetMatrix(Center);
            return tmp;
        };

        private MyMatrix4x4 OffsetMatrix(Vector3 offset)
         => new MyMatrix4x4
                ( 1,               0,        0, 0,
                  0,               1,        0, 0,
                  0,               0,        1, 0,
                  offset.X, offset.Y, offset.Z, 1);
        
        private MyMatrix4x4 RotateX { 
            get => new MyMatrix4x4
                 (1, 0, 0, 0,
                  0, Cos(_angleX), Sin(_angleX), 0,
                  0, -Sin(_angleX), Cos(_angleX), 0,
                  0, 0, 0, 1);
        }
        private MyMatrix4x4 RotateY
        {
            get => new MyMatrix4x4
                 (Cos(_angleY), 0, -Sin(_angleY), 0,
                  0, 1, 0, 0,
                  Sin(_angleY), 0 , Cos(_angleY), 0,
                  0, 0, 0, 1);
        }

        private MyMatrix4x4 RotateZ
        {
            get => new MyMatrix4x4
                 (Cos(_angleZ), Sin(_angleZ), 0, 0,
                  -Sin(_angleZ), Cos(_angleZ), 0, 0,
                  0, 0, 1, 0,
                  0, 0, 0, 1);
        }
        #endregion
    }
}
