using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class Offset3DTransformation : Transformation
    {
        #region Variables
        private float _dX = 0;
        private float _dY = 0;
        private float _dZ = 0;
        #endregion

        #region Propreties
        public float dX
        {
            get { return _dX; }
            set
            {
                _dX = value;
                OnPropertyChanged("dX");
            }
        }

        public float dY
        {
            get { return _dY; }
            set
            {
                _dY = value;
                OnPropertyChanged("dY");
            }
        }
        public float dZ
        {
            get { return _dZ; }
            set
            {
                _dZ = value;
                OnPropertyChanged("dZ");
            }
        }
        #endregion

        MyMatrix4x4 Offset { 
            get => new MyMatrix4x4
                (1, 0, 0, 0,
                  0, 1, 0, 0,
                  0, 0, 1, 0,
                  dX, dY,dZ, 1);
        }
        public override Func<Vector3, Vector3> Transform => p => p * Offset;
    }
}
