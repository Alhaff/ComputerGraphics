using CooridnateGrid.ExtensionsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    public class Bezier5Curve : DrawnObject
    {
        private List<PointOnPlane> _startEndPoints;
        private List<PointOnPlane> _middlePoints;

        public List<PointOnPlane> StartEndPoints
        {
            get { return _startEndPoints; }

            set {
                    if (value.Count == 2)
                    {
                    _startEndPoints = value;
                    }
                 }
        }

        public List<PointOnPlane> MiddlePoints
        {
            get { return _middlePoints; }

            set
            {
                if (value.Count == 4)
                {
                    _middlePoints = value;
                }
            }
        }

        public Bezier5Curve(List<PointOnPlane> startEndPoints, List<PointOnPlane> middlePoints)
        {

            StartEndPoints = startEndPoints;
            MiddlePoints = middlePoints;
        }

        public Bezier5Curve(List<PointOnPlane> startEndPoints)
        {
            StartEndPoints = startEndPoints;
            MiddlePoints = GetDefaultMiddlePoints(StartEndPoints).ToList();
        }


        private IEnumerable<PointOnPlane> GetDefaultMiddlePoints(List<PointOnPlane> startEnd)
        {
           
            var diff = (startEnd[1].Center - startEnd[0].Center) /5;
            var b = new PointOnPlane(startEnd[0].plane, startEnd[0].Center + diff);
            yield return b;
            var c = new PointOnPlane(b.plane, b.Center + diff);
            yield return c;
            var d = new PointOnPlane(c.plane, c.Center + diff);
            yield return d;
            var e = new PointOnPlane(d.plane, d.Center + diff);
            yield return e;
        }


        public Vector3 Bezier5CurvePoint(double t)
        {
            if(t >= 0 && t <= 1)
            {
                var res = (float)Math.Pow(1 - t, 5) * StartEndPoints[0].Center +
                         5 * (float)Math.Pow(t, 1) * (float)Math.Pow(1 - t, 4) * MiddlePoints[0].Center +
                         10 * (float)Math.Pow(t, 2) * (float)Math.Pow(1 - t, 3) * MiddlePoints[1].Center +
                         10 * (float)Math.Pow(t, 3) * (float)Math.Pow(1 - t, 2) * MiddlePoints[2].Center +
                         5 * (float)Math.Pow(t, 4) * (float)Math.Pow(1 - t, 1) * MiddlePoints[3].Center +
                         (float)Math.Pow(t, 5) * StartEndPoints[1].Center;
                return new Vector3(res.X, res.Y, 1);
            }
            return Vector3.Zero;
        }
        public IEnumerable<Vector3> GetBezier5FragmentCountour()
        {
            for (double t = 0; t <= 1; t += 0.025)
            {
                yield return Bezier5CurvePoint(t);
            }
            yield return StartEndPoints[1].Center;
        }

        public IEnumerable<Vector3> GetHexagonCountour()
        {
            yield return StartEndPoints[0].Center;
            yield return MiddlePoints[0].Center;
            yield return MiddlePoints[1].Center;
            yield return MiddlePoints[2].Center;
            yield return MiddlePoints[3].Center;
            yield return StartEndPoints[1].Center;
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            yield return GetBezier5FragmentCountour();
        }
    }
}
