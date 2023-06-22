using CooridnateGrid.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CoordinateGridTest
{
    [TestFixture]
    public class ProjectiveTransformationTest
    {
        public static IEnumerable<Vector3[]> GetData()
        {
            var tr = new ProjectiveTransformation();
            tr.R0 = new Vector3(0, 0, 1000);
            tr.Rx = new Vector3(10, 0, 40);
            tr.Ry = new Vector3(0, 20, 10);
            for (int x = -30; x <= 30; x++)
            {
                for (int y = -20; y <= 20; y++)
                {
                    var start = new Vector3(x, y, 1);
                    var res = tr.Transform(start);
                    yield return new Vector3[] { start, res };
                }
            }
        }

        [TestCase(0, 0,0,0)]
        [TestCase(1, 0,0.99f,0)]
        [TestCase(0, 1,0,1.98f)]
        [TestCase(1, 1, 0.98f,1.96f)]
        [TestCase(2, 0, 1.96f,0)]
        [TestCase(0, 2,0,3.92f)]
        [TestCase(2, 1,1.94f,1.94f)]
        [TestCase(1, 2,0.97f,3.88f)]
        [TestCase(2, 2,1.92f,3.85f)]
        public void ProjectiveTransformationWorksCorrect(float x, float y,float expectedX,float expectedY)
        {
            var tr = new ProjectiveTransformation();
            tr.R0 = new System.Numerics.Vector3(0, 0, 1000);
            tr.Rx = new System.Numerics.Vector3(100, 0, 10);
            tr.Ry = new System.Numerics.Vector3(0, 200, 10);

            var res = tr.Transform(new System.Numerics.Vector3(x, y, 1));

            Assert.AreEqual(expectedX,res.X, 1E-2);
            Assert.AreEqual(expectedY, res.Y, 1E-2);
        }
    }
}
