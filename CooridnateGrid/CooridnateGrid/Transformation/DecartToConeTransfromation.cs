using CooridnateGrid.DrawnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class DecartToConeTransfromation : Transformation
    {

        private double _ellipseDenominator = 20;

        private double _oneStepZAxis = 1.0 / 100;
        public Cone Cone { get; set; }
        /// <summary>
        /// Вісь u конуса, рухається вздовж еліпса від 0 до 2П
        /// </summary>
        public double OneStepOnEllipseAxis { get => Math.PI / EllipseDenominator; }
        /// <summary>
        /// Вісь v конуса, рухається прямою від -infinity до +infinity
        /// </summary>
        public double OneStepOnZAxis 
        {
            get => _oneStepZAxis; 
            set
            {
                if (value != 0)
                {
                    _oneStepZAxis = value;
                    OnPropertyChanged("OneStepOnZAxis");
                }
            }
        }

        
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

        public override Func<Vector3, Vector3> Transform => p => 
        {
            var u = p.X * OneStepOnEllipseAxis;
            var v = p.Y * OneStepOnZAxis;
            return Cone.EllipsePoint(u, v);
        };

        public DecartToConeTransfromation(Cone cone)
        {
            Cone = cone;
        }
    }
}
