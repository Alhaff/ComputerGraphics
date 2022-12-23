using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class CentralProjectionTransformation : Transformation
    {
        private Vector3 ProjectionPoints = new Vector3(1000,0,200);

        public float XAxesPoint 
        { 
            get { 
                return ProjectionPoints.X; 
                }

            set { 
                ProjectionPoints.X = value;
                OnPropertyChanged("XAxesPoint");
            }
        }

        public float YAxesPoint
        {
            get { return ProjectionPoints.Y; }
            set
            {
                ProjectionPoints.Y = value;
                OnPropertyChanged("YAxesPoint");
            }
        }

        public float ZAxesPoint
        {
            get { return ProjectionPoints.Z; }
            set { ProjectionPoints.Z = value;
                OnPropertyChanged("ZAxesPoint");
            }
        }

        private float ProjectPoint(float point) => point == 0 ? point : -1 / point;
        public override Func<Vector3, Vector3> Transform => p => p * CentralProjectionOnXOYMatrix;

        private MyMatrix4x4 CentralProjectionOnXOYMatrix
        {
            get => new MyMatrix4x4
                (
                   1, 0, 0 , ProjectPoint(XAxesPoint),
                   0, 1, 0 , ProjectPoint(YAxesPoint),
                   0, 0, 0 , ProjectPoint(ZAxesPoint),
                   0, 0, 0 , 1
                );
        }
    }
}
