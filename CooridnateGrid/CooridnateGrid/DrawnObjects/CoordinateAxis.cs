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

namespace CooridnateGrid.DrawnObjects
{
    public class CoordinateAxis : DrawnObject, IDrawingSelf
    {
        private int _cellAmountOnAbscissaAxe;
        private int _cellAmountOnOrdinateAxe;

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

        private Vector3 Point(int x, int y) => TransformMe(new Vector3(x,  y, 1));
        private Vector3 Point(double x, double y) => TransformMe(new Vector3((float)x, (float)y,1));
        private IEnumerable<Vector3> GetAbscissaLine(int y)
        {
            for (int x = -CellAmountOnAbscissaAxe / 2; x <= CellAmountOnAbscissaAxe / 2; x++)
            {
                yield return Point(x, y);
            }
        }
        private IEnumerable<Vector3> GetOrdinateLine(int x)
        {
            for (int y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            {
                yield return Point(x, y);
            }
        }
        protected override IEnumerable<IEnumerable<Vector3>> ContourPoints()
        {
            for(var x = -CellAmountOnAbscissaAxe /2; x <= CellAmountOnAbscissaAxe/2; x++)
            {
                if(x != 0)
                    yield return GetOrdinateLine(x);
            }

            for (var y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            {
                if (y != 0)
                    yield return GetAbscissaLine(y);
            }
        }
        private void DrawOrdinateAndAbscissaAxes(CoordinatePlane.MyPlane pl)
        {
            pl.DrawObj(this);

            foreach (var points in GetAbscissaLine(0).Select(point => pl.ToBitmapCoord(TransformMe(point)))
                                              .LineCreator())
            {
                pl.WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                           (int)points.Item2.X, (int)points.Item2.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            }

            foreach (var points in GetOrdinateLine(0).Select(point => pl.ToBitmapCoord(TransformMe(point)))
                                              .LineCreator())
            {
                pl.WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                           (int)points.Item2.X, (int)points.Item2.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            }
            DrawElements(pl);
        }
        private void DrawElements(CoordinatePlane.MyPlane pl)
        {
            var abscissaEnd = pl.ToBitmapCoord(TransformMe(Point(CellAmountOnAbscissaAxe/2, 0)));
            var ordinateEnd = pl.ToBitmapCoord(TransformMe(Point(0, CellAmountOnOrdinateAxe/2)));
            var k = 0.6;
            var k1 = 0.4;
            var abscissaUp = pl.ToBitmapCoord(TransformMe(Point(CellAmountOnAbscissaAxe / 2 -k, k1)));
            var abscissaDown = pl.ToBitmapCoord(TransformMe(Point(CellAmountOnAbscissaAxe / 2 -k, -k1)));
            var ordinateRight = pl.ToBitmapCoord(TransformMe(Point(k1, CellAmountOnOrdinateAxe / 2 - k)));
            var ordinateLeft = pl.ToBitmapCoord(TransformMe(Point(-k1, CellAmountOnOrdinateAxe / 2 - k)));
            var temp1 = pl.ToBitmapCoord(TransformMe(Point(1,0.3)));
            var temp2 = pl.ToBitmapCoord(TransformMe(Point(1, -0.3)));

            pl.WrBitmap.DrawLine((int)abscissaEnd.X, (int)abscissaEnd.Y, (int)abscissaUp.X,
                (int)abscissaUp.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            pl.WrBitmap.DrawLine((int)abscissaEnd.X, (int)abscissaEnd.Y, (int)abscissaDown.X,
              (int)abscissaDown.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
           
            pl.WrBitmap.DrawLine((int)ordinateEnd.X, (int)ordinateEnd.Y, (int)ordinateRight.X,
               (int)ordinateRight.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            pl.WrBitmap.DrawLine((int)ordinateEnd.X, (int)ordinateEnd.Y, (int)ordinateLeft.X,
              (int)ordinateLeft.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));

            pl.WrBitmap.DrawLine((int)temp1.X, (int)temp1.Y, (int)temp2.X,
               (int)temp2.Y, System.Windows.Media.Color.FromRgb(255, 0, 0));
        }

        private void DrawText(CoordinatePlane.MyPlane pl)
        {
            var wr = pl.WrBitmap;
            var w = wr.PixelWidth;
            var h = wr.PixelHeight;
            var stride = wr.BackBufferStride;
            var pixelPtr = wr.BackBuffer;
            var bm2 = new Bitmap(w, h, stride, CoordinatePlane.MyPlane.ConvertPixelFormat(wr.Format), pixelPtr);
            var xPoint = pl.ToBitmapCoord(TransformMe(Point(CellAmountOnAbscissaAxe / 2 - 1.5, 0 - 0.5)));
            var yPoint = pl.ToBitmapCoord(TransformMe(Point(0 + 0.5, CellAmountOnOrdinateAxe / 2 - 0.5)));
            var zeroPoint = pl.ToBitmapCoord(TransformMe(Point(0 + 0.1, 0 + 0.1)));
            var onePoint = pl.ToBitmapCoord(TransformMe(Point(1 + 0.1, 0 + 0.1)));
            wr.Lock();
            using (var g = Graphics.FromImage(bm2))
            {
                g.DrawString("X", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, xPoint.X, xPoint.Y);
                g.DrawString("Y", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, yPoint.X, yPoint.Y);
                g.DrawString("0", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, zeroPoint.X, zeroPoint.Y);
                g.DrawString("1", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, onePoint.X, onePoint.Y);
            }
            wr.AddDirtyRect(new Int32Rect(0, 0, 200, 100));
            wr.Unlock();
        }
        public void Draw(CoordinatePlane.MyPlane pl)
        {
            DrawOrdinateAndAbscissaAxes(pl);
            DrawText(pl);
        }
    }
}
