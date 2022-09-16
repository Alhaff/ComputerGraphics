using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class AffineTransformation : Transformation
    {
        private Vector3 _r0 = new Vector3(0,0,1);
        private Vector3 _rx = new Vector3(1,0,0);
        private Vector3 _ry = new Vector3(0,1,0);

        public Vector3 R0
        {
            get { return _r0; }
            set { 
                _r0 = value;
                OnPropertyChanged("R0");
            }
        }

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

        public override Func<Vector3, Vector3> Transform => v => R0 + (Rx * v.X) + (Ry * v.Y);
    }
}
