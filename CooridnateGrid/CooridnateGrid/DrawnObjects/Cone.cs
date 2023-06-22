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
        #region Variables

        private float _A = 20;

        private float _B = 10;

        private float _C = 35f;

        private int height = 36;

        private double _ellipseDenominator = 20;

        private double _zStep = 1;

        private Vector3 _center = new Vector3(0, 0, 0);
        #endregion

        #region Propreties
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

        public float B
        {
            get { return _B; }
            set
            {
                _B = value;
                OnPropertyChanged("B");
            }
        }

        public float C
        {
            get { return _C; }
            set
            {
                if (value != 0)
                {
                    _C = value;
                    OnPropertyChanged("C");
                }
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// Вісь v конуса, рухається прямою від -infinity до +infinity
        ///  <br/>
        ///  Ділимо на параметр C щоб одиничні відрізки збігалися з вісю Z,
        ///  тобто переміщення точки з v = 0 до v = 1 змістить її відповідно від z = 0 до z =1
        /// </summary>
        public double OneStepOnZAxis
        {
            get => ZStep / C;

        }

        /// <summary>
        /// Вісь u конуса, рухається прямою від 0 до 2П
        /// </summary>
        public double OneStepOnEllipseAxis { get => Math.PI / EllipseDenominator; }

        /// <summary>
        /// Дільник на який ми ділимо П, для визначення одиничного кроку на осі u
        /// </summary>
        public double EllipseDenominator
        {
            get => _ellipseDenominator;
            set
            {
                if (_ellipseDenominator != 0)
                {
                    _ellipseDenominator = value;
                    OnPropertyChanged("EllipseDenominator");
                }
            }
        }
      
        

        public double ZStep
        {
            get => _zStep;
            set
            {
                if (_zStep != 0)
                {
                    _zStep = value;
                    OnPropertyChanged("EllipseDenominator");
                }
            }
        }
        #endregion
        public Vector3 EllipsePoint(double u, double v)
        {
            float x = Center.X + (float)(A * v * Math.Cos(u));
            float y = Center.Y + (float)(B * v * Math.Sin(u));
            float z = Center.Z + (float)(C * v);
            return new Vector3(x, y, z);
        }

        private IEnumerable<Vector3> GetEllipsePoint(double v)
        {
            
            for(double u = 0; u <= 2* Math.PI; u+= OneStepOnEllipseAxis)
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
            for (double z = -Height* OneStepOnZAxis; z <= Height* OneStepOnZAxis; z += OneStepOnZAxis)
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
                z += OneStepOnZAxis;
                if (z <= Height * OneStepOnZAxis)
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
