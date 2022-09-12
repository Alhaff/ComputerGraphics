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

        private Vector2 Point(int x, int y) => new Vector2(x,  y);
        private Vector2 Point(double x, double y) => new Vector2((float)x, (float)y);
        private IEnumerable<Vector2> GetAbscissaLine(int y)
        {
            for (int x = -CellAmountOnAbscissaAxe / 2; x <= CellAmountOnAbscissaAxe / 2; x++)
            {
                yield return Point(x, y);
            }
        }
        private IEnumerable<Vector2> GetOrdinateLine(int x)
        {
            for (int y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            {
                yield return Point(x, y);
            }
        }
        public override IEnumerable<IEnumerable<Vector2>> GetContourPoints()
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
        private void DrawOrdinateAndAbscissaAxes(CoordinatePlane.Plane pl)
        {
            pl.DrawObj(this);

            foreach (var points in GetAbscissaLine(0).Select(point => pl.ToBitmapCoord(TransformFunctions(point)))
                                              .LineCreator())
            {
                pl.WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                           (int)points.Item2.X, (int)points.Item2.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            }

            foreach (var points in GetOrdinateLine(0).Select(point => pl.ToBitmapCoord(TransformFunctions(point)))
                                              .LineCreator())
            {
                pl.WrBitmap.DrawLine((int)points.Item1.X, (int)points.Item1.Y,
                           (int)points.Item2.X, (int)points.Item2.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            }
            DrawElements(pl);
        }
        private void DrawElements(CoordinatePlane.Plane pl)
        {
            var abscissaEnd = pl.ToBitmapCoord(TransformFunctions(Point(CellAmountOnAbscissaAxe/2, 0)));
            var ordinateEnd = pl.ToBitmapCoord(TransformFunctions(Point(0, CellAmountOnOrdinateAxe/2)));
            var k = 0.6;
            var k1 = 0.4;
            var abscissaUp = pl.ToBitmapCoord(TransformFunctions(Point(CellAmountOnAbscissaAxe / 2 -k, k1)));
            var abscissaDown = pl.ToBitmapCoord(TransformFunctions(Point(CellAmountOnAbscissaAxe / 2 -k, -k1)));
            var ordinateRight = pl.ToBitmapCoord(TransformFunctions(Point(k1, CellAmountOnOrdinateAxe / 2 - k)));
            var ordinateLeft = pl.ToBitmapCoord(TransformFunctions(Point(-k1, CellAmountOnOrdinateAxe / 2 - k)));
            var temp1 = pl.ToBitmapCoord(TransformFunctions(Point(1,0.3)));
            var temp2 = pl.ToBitmapCoord(TransformFunctions(Point(1, -0.3)));

            pl.WrBitmap.DrawLine((int)abscissaEnd.X, (int)abscissaEnd.Y, (int)abscissaUp.X,
                (int)abscissaUp.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            pl.WrBitmap.DrawLine((int)abscissaEnd.X, (int)abscissaEnd.Y, (int)abscissaUp.X,
              (int)abscissaDown.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
           
            pl.WrBitmap.DrawLine((int)ordinateEnd.X, (int)ordinateEnd.Y, (int)ordinateRight.X,
               (int)ordinateRight.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));
            pl.WrBitmap.DrawLine((int)ordinateEnd.X, (int)ordinateEnd.Y, (int)ordinateLeft.X,
              (int)ordinateRight.Y, System.Windows.Media.Color.FromRgb(0, 0, 255));

            pl.WrBitmap.DrawLine((int)temp1.X, (int)temp1.Y, (int)temp2.X,
               (int)temp2.Y, System.Windows.Media.Color.FromRgb(255, 0, 0));
        }

        private void DrawText(CoordinatePlane.Plane pl)
        {
            var wr = pl.WrBitmap;
            var w = wr.PixelWidth;
            var h = wr.PixelHeight;
            var stride = wr.BackBufferStride;
            var pixelPtr = wr.BackBuffer;
            var bm2 = new Bitmap(w, h, stride, CoordinatePlane.Plane.ConvertPixelFormat(wr.Format), pixelPtr);

            wr.Lock();
            // you might wanna use this in combination with Lock / Unlock, AddDirtyRect, Freeze
            // before you write to the shared Ptr
            using (var g = Graphics.FromImage(bm2))
            {
                g.DrawString("X", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, (float)(wr.Width - 20f), (float)(wr.Height / 2 + 5f));
                g.DrawString("Y", new Font("Times New Roman", 12), System.Drawing.Brushes.Blue, (float)(wr.Width / 2 + 10f), 5f);
                g.DrawString("0", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, (float)(wr.Width / 2 + 2f), (float)(wr.Height / 2 + 2f));
                g.DrawString("1", new Font("Times New Roman", 10), System.Drawing.Brushes.Blue, (float)(wr.Width / 2 + 22f), (float)(wr.Height / 2 + 2f));
            }
            wr.AddDirtyRect(new Int32Rect(0, 0, 200, 100));
            wr.Unlock();
        }
        public void Draw(CoordinatePlane.Plane pl)
        {
            DrawOrdinateAndAbscissaAxes(pl);
            DrawText(pl);
        }
    }
}
