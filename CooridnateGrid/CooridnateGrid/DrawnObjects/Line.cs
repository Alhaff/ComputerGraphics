﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class Line : DrawnObject
    {
        #region Variables
        private Vector3 _start;
        private Vector3 _end;
        #endregion

        #region Properties

        public Vector3 Start
        {
            get { return _start; }
            set { 
                    _start = value;
                    OnPropertyChanged("Start");
                }
        }

        public Vector3 End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
            }
        }
        #endregion

        public Line(Vector3 start, Vector3 end, Color color)
        {
            Start = start;
            End = end;
        }

        internal IEnumerable<Vector3> GetLinePoint()
        {
            yield return Start;
            yield return End;
        }
        public override IEnumerable<IEnumerable<Vector3>> GetContourPoints()
        {
            yield return GetLinePoint();
        }

        static public Vector3 GetLineEndPoint(Vector3 startPoint, float length, double angle = 0)
        {
            return startPoint + (new Vector3(length, 0, 0)).Rotate(angle);
        }
    }
}
