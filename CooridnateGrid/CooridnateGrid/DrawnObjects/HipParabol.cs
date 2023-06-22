using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    public class HipParabol : DrawnObject
    {
        private float _A = 0.5f;

        public float A
        {
            get { return _A; }
            set
            {
                _A = value;
                OnPropertyChanged("A");
            }
        }

        private float _B = 0.5f;

        public float B
        {
            get { return _B; }
            set
            {
                _B = value;
                OnPropertyChanged("B");
            }
        }

        private float _C = 0.3f;

        public float C
        {
            get { return _C; }
            set
            {
                _C = value;
                OnPropertyChanged("C");
            }
        }

        private double height = 1000;

        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        private double _width = 7;

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }


        public Vector3 HipPoint(double u, double v)
        {
            float x = (float)(A * v * Math.Cosh(u));
            float y = (float)(B * v * Math.Sinh(u));
            float z = (float)(C * u * u);
            return new Vector3(x, y, z);
        }

        private IEnumerable<Vector3> GetHipPoint(double v)
        {

            for (double u = -Width / 4; u <= Width/4; u += 2)
            {
                yield return HipPoint(u, v);
            }
            
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
            var step = 2;
            for (double z = -Height /4; z <= Height /4; z += step)
            {
                elipse1 = GetHipPoint(z).ToList();
                if (elipse2 != null)
                {
                    foreach (var line in getLines(elipse2, elipse1))
                    {
                        yield return line;
                    }
                }
                yield return elipse1;
                z += step;
                if (z <= Height / 4)
                {
                    elipse2 = GetHipPoint(z).ToList();
                    var lines = getLines(elipse1, elipse2).ToList();

                    foreach (var line in lines)
                    {
                        yield return line;
                    }
                    yield return elipse2;
                }
            }

        }
    }
}
