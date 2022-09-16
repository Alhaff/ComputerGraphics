using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class ProjectiveTransformation : Transformation
    {
        #region Variables
        private Vector3 _r0 = new Vector3(0, 0, 1000);
        private Vector3 _rx = new Vector3(800,50,1);
        private Vector3 _ry = new Vector3(90,800,1);
        #endregion

        #region Propreties
        public Vector3 Rx
        {
            get { return _rx; }
            set {
                _rx = value;
                OnPropertyChanged("Rx");
            }
        }

        public Vector3 Ry
        {
            get { return _ry; }
            set {
                _ry = value;
                OnPropertyChanged("Ry");
            }
        }

        public Vector3 R0
        {
            get { return _r0; }
            set { 
                _r0 = value;
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
        {
            var tmp = new Vector3(v.X, v.Y, 1);
            return tmp * ProjectiveMatrix;
        
         };
        //{
        //    var r0 = new Vector3(Center.X, Center.Y, 1);
        //    var w0 = Center.Z;
        //    var rx = new Vector3(XEnd.X, XEnd.Y, 1);
        //    var wx = XEnd.Z;
        //    var ry = new Vector3(YEnd.X, YEnd.Y, 1);
        //    var wy = YEnd.Z;
        //    return  ( r0 * w0 + rx * wx * v.X + ry * wy * v.Y) / ( w0 + wx* v.X + wy *v.Y);
        //};
    }
}
