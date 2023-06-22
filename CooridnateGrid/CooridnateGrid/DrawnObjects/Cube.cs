using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class Cube : DrawnObject
    {
        float Len = 20;
        Vector3 BottomCornerPoint = new Vector3(0, 0, 1);
  

        private IEnumerable<Vector3> XYPoints(Vector3 point, Vector3 direction)
        {
            yield return point;
            yield return point + new Vector3(direction.X, 0, 0);
            yield return point + new Vector3(direction.X, direction.Y, 0);
            yield return point + new Vector3(0, direction.Y,0);
            yield return point;
        }
        private IEnumerable<Vector3> ZYPoints(Vector3 point, Vector3 direction)
        {
            yield return point;
            yield return point + new Vector3(0, direction.Y,0);
            yield return point + new Vector3(0, direction.Y, direction.Z);
            yield return point + new Vector3(0, 0, direction.Z);
            yield return point;
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            MyColor = Colors.Red;
            yield return XYPoints(BottomCornerPoint, new Vector3(Len, Len, 0));
            MyColor = Colors.Blue;
            yield return XYPoints(BottomCornerPoint + new Vector3(0,0,Len), new Vector3(Len, Len, 0));
            MyColor = Colors.Purple;
            yield return ZYPoints(BottomCornerPoint, new Vector3(0, Len, Len));
            MyColor = Colors.Orange;
            yield return ZYPoints(BottomCornerPoint + new Vector3(Len,0,0), new Vector3(0, Len, Len));
        }
    }
}
