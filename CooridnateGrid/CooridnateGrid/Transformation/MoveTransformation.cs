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
        #region Variables
        private float _dX = 0;
        private float _dY = 0;
        #endregion

        #region Propreties
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
        #endregion

        public override Func<Vector3, Vector3> Transform => v => v + new Vector3(dX, dY, 0);    
    }
}
