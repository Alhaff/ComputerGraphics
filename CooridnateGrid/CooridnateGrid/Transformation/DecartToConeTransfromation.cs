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
        public Cone Cone { get; set; }
     
        public override Func<Vector3, Vector3> Transform => p => 
        {
            var u = p.X * Cone.OneStepOnEllipseAxis;
            var v = p.Y * Cone.OneStepOnZAxis;
            return Cone.EllipsePoint(u, v);
        };

        public DecartToConeTransfromation(Cone cone)
        {
            Cone = cone;
        }
    }
}
