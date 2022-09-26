using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CooridnateGrid.ExtensionsClasses;

namespace CooridnateGrid.DrawnObjects
{
    public class PointOnPlane : DrawnObject
    {
        #region Variables
            private Circle _point;
        #endregion

        #region Propreties
        public Vector3 Center
        {
            get { return _point.Center; }
            set 
            { 
                _point.Center = value;
                OnPropertyChanged("Center");
            }
        }
        #endregion

        #region Constructors
        public PointOnPlane(float x, float y, float z = 1f)
        {
            MyColor = System.Windows.Media.Color.FromRgb(220, 0, 0);
            _point = new Circle( new Vector3(x, y, z), 0.3f, MyColor);
            IsPlaneTransfromMe = false;
        }
        public PointOnPlane(Vector3 vector)
        {
            MyColor = System.Windows.Media.Color.FromRgb(220, 0, 0);
            _point = new Circle(vector, 0.3f, MyColor);
            IsPlaneTransfromMe = false;
        }
        #endregion

        #region Methods
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            return _point.GetContourPoints();
        }
        #endregion
    }
}
