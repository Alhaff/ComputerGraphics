using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CooridnateGrid.CoordinatePlane;
using CooridnateGrid.ExtensionsClasses;

namespace CooridnateGrid.DrawnObjects
{
    public class CoordinateAxis : DrawnObject, IDrawingSelf
    {
        #region Variables
        private int _cellAmountOnAbscissaAxe;
        private int _cellAmountOnOrdinateAxe;
        #endregion

        #region Propreties
        public int CellAmountOnAbscissaAxe {
            get { return _cellAmountOnAbscissaAxe; }
            set { _cellAmountOnAbscissaAxe = value;
                OnPropertyChanged("Width");
            }
        }

        public int CellAmountOnOrdinateAxe
        {
            get { return _cellAmountOnOrdinateAxe; }
            set { _cellAmountOnOrdinateAxe = value;
                OnPropertyChanged("Height");
            }
        }
        #endregion

        #region Constructors

        public CoordinateAxis(MyPlane pl)
        {
            CellAmountOnAbscissaAxe = (int)(pl.BitmapWidth) / pl.StepInPixels;
            CellAmountOnOrdinateAxe = (int)(pl.BitmapHeight) / pl.StepInPixels;
            MyColor = System.Windows.Media.Color.FromRgb(128, 128, 128);
        }
        /// <summary>
        ///  Створює об'єкт кооординатних вісей
        /// </summary>
        /// <param name="cellAmountOnAbscissaAxe"> Загальна кількість клітинок на осі абсцис</param>
        /// <param name="cellAmountOnOrdinateAxe">Загальна кількість клітинок на осі ординат</param>
        public CoordinateAxis(int cellAmountOnAbscissaAxe, int cellAmountOnOrdinateAxe)
        {
           CellAmountOnAbscissaAxe = cellAmountOnAbscissaAxe;
           CellAmountOnOrdinateAxe = cellAmountOnOrdinateAxe;
           MyColor = System.Windows.Media.Color.FromRgb(128, 128, 128);
        }
        /// <summary>
        ///  Створює об'єкт кооординатних вісей
        /// </summary>
        /// <param name="cellAmountOnAbscissaAxe"> Загальна кількість клітинок на осі абсцис</param>
        /// <param name="cellAmountOnOrdinateAxe">Загальна кількість клітинок на осі ординат</param>
        /// <param name="color">Колір вісей</param>
        public CoordinateAxis(int cellAmountOnAbscissaAxe, int cellAmountOnOrdinateAxe, System.Windows.Media.Color color)
        {
            CellAmountOnAbscissaAxe = cellAmountOnAbscissaAxe;
            CellAmountOnOrdinateAxe = cellAmountOnOrdinateAxe;
            MyColor = color;
        }
        #endregion

        #region Methods
        private Vector3 Point(int x, int y) => TransformMe(new Vector3(x,  y, 1));
        private Vector3 Point(double x, double y) => TransformMe(new Vector3((float)x, (float)y,1));
        private IEnumerable<Vector3> GetAbscissaLine(int y)
        {
            //for (int x = -CellAmountOnAbscissaAxe / 2; x <= CellAmountOnAbscissaAxe / 2; x++)
            //{
            //    yield return Point(x, y);
            //}
            //Uncomment bottom lines and comment uper to reduce points amount, which optimize drawing process
            yield return Point(-CellAmountOnAbscissaAxe / 2, y);
            yield return Point(CellAmountOnAbscissaAxe / 2, y);
        }
        private IEnumerable<Vector3> GetOrdinateLine(int x)
        {
            //for (int y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            //{
            //    yield return Point(x, y);
            //}
            //Uncomment bottom lines and comment uper to reduce points amount, which optimize drawing process
            yield return Point(x, -CellAmountOnOrdinateAxe / 2);
            yield return Point(x, CellAmountOnOrdinateAxe / 2);
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            throw new NotImplementedException("Use IDrawingSelf, to draw this figure");
        }
        private void DrawOrdinateAndAbscissaGrid(CoordinatePlane.MyPlane pl, Graphics g)
        {
            Func<Vector3, Vector3> ToBitmap = IsPlaneTransfromMe ? pl.ToBitmapCoord : pl.ToBitmapCoordWithoutTransform;
            for (var x = -CellAmountOnAbscissaAxe / 2; x <= CellAmountOnAbscissaAxe / 2; x++)
            {
                foreach (var points in GetOrdinateLine(x).Select(point => ToBitmap(point))
                                                         .LineCreator())
                {
                    System.Windows.Media.Color color = x != 0 ? MyColor : System.Windows.Media.Color.FromRgb(0, 0, 255);
                    pl.DrawLine(g, points, color);
                }
            }

            for (var y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            {
                foreach (var points in GetAbscissaLine(y).Select(point => ToBitmap(point))
                                                         .LineCreator())
                {
                    System.Windows.Media.Color color = y != 0 ? MyColor : System.Windows.Media.Color.FromRgb(0, 0, 255);
                    pl.DrawLine(g, points, color);
                }
            }
            DrawElements(pl, g);
        }
        private void DrawElements(CoordinatePlane.MyPlane pl, Graphics g)
        {
            Func<Vector3, Vector3> ToBitmap = IsPlaneTransfromMe ? pl.ToBitmapCoord : pl.ToBitmapCoordWithoutTransform;
            var abscissaEnd = ToBitmap(Point(CellAmountOnAbscissaAxe/2, 0));
            var ordinateEnd = ToBitmap(Point(0, CellAmountOnOrdinateAxe/2));
            var k = 0.6;
            var k1 = 0.4;
            var abscissaArrowUp = ToBitmap(Point(CellAmountOnAbscissaAxe / 2 -k, k1));
            var abscissaArrowDown = ToBitmap(Point(CellAmountOnAbscissaAxe / 2 -k, -k1));
            var ordinateArrowRight = ToBitmap(Point(k1, CellAmountOnOrdinateAxe / 2 - k));
            var ordinateArrowLeft = ToBitmap(Point(-k1, CellAmountOnOrdinateAxe / 2 - k));
            var temp1 = ToBitmap(Point(1,0.3));
            var temp2 = ToBitmap(Point(1, -0.3));
            var temp3 = ToBitmap(Point(0.3,1));
            var temp4 = ToBitmap(Point(-0.3,1));
            var color1 = System.Windows.Media.Color.FromRgb(0, 0, 255);
            var color2 = System.Windows.Media.Color.FromRgb(255, 0, 0);
            pl.DrawLine(g, Tuple.Create(abscissaEnd, abscissaArrowUp), color1);
            pl.DrawLine(g, Tuple.Create(abscissaEnd, abscissaArrowDown), color1);
            pl.DrawLine(g, Tuple.Create(ordinateEnd, ordinateArrowRight), color1);
            pl.DrawLine(g, Tuple.Create(ordinateEnd, ordinateArrowLeft), color1);
            pl.DrawLine(g, Tuple.Create(temp1, temp2), color2);
            pl.DrawLine(g, Tuple.Create(temp3, temp4), color2);
        }

        private void DrawText(CoordinatePlane.MyPlane pl, Graphics g)
        {
            Func<Vector3, Vector3> ToBitmap = IsPlaneTransfromMe ? pl.ToBitmapCoord : pl.ToBitmapCoordWithoutTransform;
            var xPoint = ToBitmap(Point(CellAmountOnAbscissaAxe / 2 - 1.5, 0 - 0.5));
            var yPoint = ToBitmap(Point(0 + 0.5, CellAmountOnOrdinateAxe / 2 - 0.5));
            var zeroPoint = ToBitmap(Point(0 + 0.1, 0 + 0.1));
            var onePointX = ToBitmap(Point(1 + 0.1, 0 + 0.1));
            var onePointY = ToBitmap(Point(0 + 0.1, 1 + 0.1));
            g.DrawString("X", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, xPoint.X, xPoint.Y);
            g.DrawString("Y", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, yPoint.X, yPoint.Y);
            g.DrawString("0", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, zeroPoint.X, zeroPoint.Y);
            g.DrawString("1", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, onePointX.X, onePointX.Y);
            g.DrawString("1", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, onePointY.X, onePointY.Y);           
        }
        public void Draw(CoordinatePlane.MyPlane pl, Graphics g)
        {
            DrawOrdinateAndAbscissaGrid(pl, g);
            DrawText(pl,g);
        }
        #endregion
    }
}
