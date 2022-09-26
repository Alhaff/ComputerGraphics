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
        private PointOnPlane _r0 = new PointOnPlane(new Vector3(0, 0, 1000));
        private PointOnPlane _rx = new PointOnPlane(new Vector3(10,0,20));
        private PointOnPlane _ry = new PointOnPlane(new Vector3(0,100,10));
        #endregion

        #region Propreties
        public PointOnPlane R0Point { get => _r0; }
        public PointOnPlane RxPoint { get => _rx; }
        public PointOnPlane RyPoint { get => _ry; }
        public Vector3 Rx
        {
            get { return _rx.Center; }
            set
            {
                _rx.Center = value;
                OnPropertyChanged("Rx");
            }
        }
        public Vector3 Ry
        {
            get { return _ry.Center; }
            set {
                _ry.Center = value;
                OnPropertyChanged("Ry");
            }
        }

        public Vector3 R0
        {
            get { return _r0.Center; }
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
