using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public class TransformationConnector : Transformation
    {
        public readonly Transformation[] Transformations;

        public TransformationConnector(params Transformation[] transformations)
        {
            Transformations = transformations;
        }

        public override Func<Vector3, Vector3> Transform => v => 
        {
            Vector3 temp = v;
            foreach(var transformation in Transformations)
            {
                temp = transformation.Transform(temp);
            }
            return temp;
        };
    }
}
