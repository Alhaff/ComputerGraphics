using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    public class Axes3d : DrawnObject
    {
        private int cellAmount = 100;
        Color XAxisColor { get; set; } = Colors.Red;
        Color YAxisColor { get; set; } = Colors.Green;
        Color ZAxisColor { get; set; } = Colors.Blue;

        private Vector3 _center = new Vector3(0,0,0);

        public Vector3 Center
        {
            get { return _center; }
            set 
            {
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public int CellAmount
        {
            get { return cellAmount; }
            set { cellAmount = value; }
        }
        public Axes3d()
        {

        }

        public Axes3d(Color x, Color y, Color z)
        {
            XAxisColor = x;
            YAxisColor = y;
            ZAxisColor = z;
        }
        private IEnumerable<Vector3> Axis(char letter)
        {
           
                switch(letter)
                {
                    case 'X':
                    case 'x':
                        return new List<Vector3>() { Center, Center + new Vector3(CellAmount, 0, 0) };
                    break;
                    case 'Y':
                    case 'y':
                        return new List<Vector3>() { Center, Center + new Vector3(0, CellAmount, 0) };
                    break;
                    case 'Z':
                    case 'z':
                         return new List<Vector3>() { Center, Center + new Vector3(0, 0, CellAmount) };
                    break;
                    default:
                        return new List<Vector3>() { Center, Center };
                    break;
            }
        }
        
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            MyColor = XAxisColor;
            yield return Axis('x');
            MyColor = YAxisColor;
            yield return Axis('y');
            MyColor= ZAxisColor;
            yield return Axis('z');
        }
    }
}
