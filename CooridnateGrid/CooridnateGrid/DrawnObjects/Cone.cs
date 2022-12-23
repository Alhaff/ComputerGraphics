using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    /// <summary>
    /// <p>Конус на основі еліпса</p>
    ///  <para> Для побудови використаємо рівняння <br/>
    ///   x = Azcos(u) <br/>
    ///   y = Bzsin(u) <br/>
    ///   z = Cz <br/>
    ///   u є [0,2П]
    /// </para>
    /// </summary>
    public class Cone : DrawnObject
    {
        private float _A = 20;

        private Vector3 _center = new Vector3(0, 0,0);

        public Vector3 Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Center");    
            }
        }
        public float A
        {
            get { return _A; }
            set 
            { 
                _A = value;
                OnPropertyChanged("A");
            }
        }

        private float _B = 10;

        public float B
        {
            get { return _B; }
            set 
            { 
                _B = value;
                OnPropertyChanged("B");
            }
        }

        private float _C = 35f;

        public float C
        {
            get { return _C; }
            set 
            { 
                _C = value;
                OnPropertyChanged("C");
            }
        }
        
        private int height = 10;

        public int Height
        {
            get { return height; }
            set 
            { 
                height = value;
                OnPropertyChanged("Height");
            }
        }

        public Vector3 EllipsePoint(double u, double v)
        {
            float x = Center.X + (float)(A * v * Math.Cos(u));
            float y = Center.Y + (float)(B * v * Math.Sin(u));
            float z = Center.Z + (float)(C * v);
            return new Vector3(x, y, z);
        }

        private IEnumerable<Vector3> GetEllipsePoint(double v)
        {
            
            for(double u = 0; u <= 2* Math.PI; u+= Math.PI/10)
            { 
                yield return EllipsePoint(u,v);
            }
            yield return EllipsePoint(0, v);
        }

        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            IEnumerable<List<Vector3>> getLines(List<Vector3> el1, List<Vector3> el2)
            {
                if (el1.Count == el2.Count)
                {
                    for (int i = 0; i < el1.Count; i++)
                    {
                        yield return new List<Vector3>() { el1[i], el2[i] };
                    }
                }
            };
            List<Vector3> elipse1 = null;
            List<Vector3> elipse2 = null;
            var step = 1 / C;
            for (double z = -Height / C; z <= Height/C; z += step)
            {
                elipse1 = GetEllipsePoint(z).ToList();
                if (elipse2 != null)
                {
                    foreach (var line in getLines(elipse2, elipse1))
                    {
                        yield return line;
                    }
                }
                yield return elipse1;
                z += step;
                if (z <= Height / C)
                {
                    elipse2 = GetEllipsePoint(z).ToList();
                    var lines = getLines(elipse1, elipse2).ToList();
                   
                    foreach (var line in lines)
                    {
                        yield return line;
                    }
                    yield return elipse2;
                }
            }
            //var res = GetEllipsePoint(1).ToList();
            //yield return res;
        }
    }
}
