using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class MoveTransformation : Transformation
    {
        private float _dX = 0;
        private float _dY = 0;

        public float dX
        {
            get { return _dX; }
            set { 
                _dX = value;
                OnPropertyChanged("dX");
                }
        }

        public float dY
        {
            get { return _dY; }
            set { 
                _dY = value;
                OnPropertyChanged("dY");
            }
        }
        private Func<Vector3, Vector3> F { get => v => v + new Vector3(dX, dY, 0); } 
        public override Func<Vector3, Vector3> Transform => F;       
    }
}
