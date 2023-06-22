using CooridnateGrid.CoordinatePlane;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CooridnateGrid.Transformation;

namespace CooridnateGrid.DrawnObjects
{
    public class ZigzagFractal : DrawnObject, IDrawingSelf
    {
        private int _iterationCount = 0;

        private Vector3 _startPoint = new Vector3(0,0,1);

        public Vector3 StartPoint
        {
            get { return _startPoint; }
            set 
            {
                _startPoint = value;
                OnPropertyChanged("StartPoint");
            }
        }

        public int IterationCount
        {
            get { return _iterationCount; }
            set 
            {
                if (value >= 0)
                {
                    _iterationCount = value;
                    OnPropertyChanged("IterationCount");
                }
            }
        }

        public int RandomSeed { get; set; } = 432;

        private int _randomGeneratorMaxValue = 100;
        
        public int RandomGeneratorMaxValue
        {
            get { return _randomGeneratorMaxValue; }
            set 
            { 
                _randomGeneratorMaxValue = value;
                OnPropertyChanged("RandomGeneratorMaxValue");
            }
        }

        public AffineTransformation First { get; set; }
            = new AffineTransformation(
                new MyMatrix3x3(
                                 -.632407f, -.614815f, 0,
                                 -.545370f,  .659259f, 0,
                                 3.840822f, 1.282321f, 1                                
                               )
                                      );
        public AffineTransformation Second { get; set; }
          = new AffineTransformation(
              new MyMatrix3x3(
                               -.036111f,  .444444f, 0,
                                .210185f,  .037037f, 0,
                               2.071081f, 8.330552f, 1
                             )
                                    );
        public float FirstProbability { get; set; } = .888128f;

        public float SecondProbability { get; set; } =.111872f;

        private IEnumerable<Vector3> ZigzagFractalPoint()
        {
            var currPoint = StartPoint;
            var randGenerator = new Random(RandomSeed);
            for(int i =0; i < IterationCount; i++)
            {
                var randCurr = randGenerator.Next(RandomGeneratorMaxValue);
                if(randCurr <= RandomGeneratorMaxValue * FirstProbability)
                {
                    currPoint = First.Transform(currPoint);
                    yield return currPoint;
                }
                else
                {
                    currPoint = Second.Transform(currPoint);
                    yield return currPoint;
                }
            }
        }
        public void Draw(MyPlane pl, Graphics g)
        {
            Func<Vector3, Vector3> ToBitmap = IsPlaneTransfromMe ? pl.ToBitmapCoord : pl.ToBitmapCoordWithoutTransform;
           foreach(var point in ZigzagFractalPoint()
                            .Select(p => ToBitmap(TransformMe(p))))
            {

                g.DrawEllipse(new Pen(
                               new SolidBrush(
                                   Color.FromArgb(MyColor.A, MyColor.R, MyColor.G, MyColor.B)
                                              )
                                      ),
                                new RectangleF(point.X, point.Y, Thickness, Thickness)
                             );
            }
        }

        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            throw new NotImplementedException();
        }
    }
}
