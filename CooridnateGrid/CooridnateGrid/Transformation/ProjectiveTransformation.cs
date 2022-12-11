using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CooridnateGrid.DrawnObjects;

namespace CooridnateGrid.Transformation
{
    public class ProjectiveTransformation : Transformation
    {
        #region Variables
        private PointOnPlane _r0;
        private PointOnPlane _rx;
        private PointOnPlane _ry;
        #endregion

        #region Propreties
        public PointOnPlane R0Point 
        { 
            get => _r0; 
            set
            {
                if (_r0 == null)
                {
                    _r0 = value;
                    _r0.PropertyChanged += (s,e) => OnPropertyChanged("R0");
                }
                _r0 = value;
                OnPropertyChanged("R0Point");
            }
        }
        public PointOnPlane RxPoint 
        { 
            get => _rx;
            set
            {
                if (_rx == null)
                {
                    _rx = value;
                    _rx.PropertyChanged += (s,e) => OnPropertyChanged("Rx");
                }
                _rx = value;
                OnPropertyChanged("RxPoint");
            }
        }
        public PointOnPlane RyPoint 
        { 
            get => _ry;
            set
            {
                if (_ry == null)
                {
                    _ry = value;
                    _ry.PropertyChanged += (s,e) => OnPropertyChanged("Ry");
                }
                _ry = value;
                OnPropertyChanged("RyPoint");
            }
        }
        public Vector3 Rx
        {
            get
            {
                if (_rx != null)
                {
                    return _rx.Center;
                }
                return Vector3.Zero;
            }
            set
            {
                _rx.Center = value;
                OnPropertyChanged("Rx");
            }
        }
        public Vector3 Ry
        {
            get
            {
                if (_ry != null)
                {
                    return _ry.Center;
                }
                return Vector3.Zero;
            }
            set {
                _ry.Center = value;
                OnPropertyChanged("Ry");
            }
        }

        public Vector3 R0
        {
            get
            {
                if (_r0 != null)
                {
                    return _r0.Center;
                }
                return Vector3.Zero;
            }
            set { 
                _r0.Center = value;
                OnPropertyChanged("R0");
            }
        }
        private MyMatrix3x3 ProjectiveMatrix
        {
            get => new MyMatrix3x3
                (
                    Rx.X * Rx.Z, Rx.Y * Rx.Z, Rx.Z,
                    Ry.X * Rx.Z, Ry.Y * Ry.Z, Ry.Z,
                    R0.X * R0.Z, R0.Y * R0.Z, R0.Z
                );
        }
        #endregion

        public override Func<Vector3, Vector3> Transform => v =>
        //{
        //    var tmp = new Vector3(v.X, v.Y, 1);
        //    var res = tmp * ProjectiveMatrix;
        //    return res;
        //};
        {
            var r0 = new Vector2(R0.X, R0.Y);
            var w0 = R0.Z;
            var rx = new Vector2(Rx.X, Rx.Y);
            var wx = Rx.Z;
            var ry = new Vector2(Ry.X, Ry.Y);
            var wy = Ry.Z;
            var tmp = (r0 * w0 + rx * wx * v.X + ry * wy * v.Y) / (w0 + wx * v.X + wy * v.Y);
            return new Vector3(tmp.X, tmp.Y, 1);
        };
    }
}
