using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CooridnateGrid.CoordinatePlane;

namespace CooridnateGrid.DrawnObjects
{
    public class CoordinateAxis : IDrawnObject
    {
        private int _cellAmountOnAbscissaAxe;
        private int _cellAmountOnOrdinateAxe;
        private Func<Vector2, Vector2> _transformFunctions;

        private Color _myColor;

        public Color MyColor
        {
            get { return _myColor; }
            set { _myColor = value; }
        }

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
        public Func<Vector2, Vector2> TransformFunctions 
        {
            get => _transformFunctions;
            set { _transformFunctions = value;
                OnPropertyChanged("TransformFunctions");
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
           MyColor = Color.FromRgb(0, 0, 0);
           TransformFunctions = v => v;
        }
        /// <summary>
        ///  Створює об'єкт кооординатних вісей
        /// </summary>
        /// <param name="cellAmountOnAbscissaAxe"> Загальна кількість клітинок на осі абсцис</param>
        /// <param name="cellAmountOnOrdinateAxe">Загальна кількість клітинок на осі ординат</param>
        /// <param name="color">Колір вісей</param>
        public CoordinateAxis(int cellAmountOnAbscissaAxe, int cellAmountOnOrdinateAxe, Color color)
        {
            CellAmountOnAbscissaAxe = cellAmountOnAbscissaAxe;
            CellAmountOnOrdinateAxe = cellAmountOnOrdinateAxe;
            MyColor = color;
            TransformFunctions = v => v;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private Vector2 Point(int x, int y) => new Vector2(x,  y);

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
        public IEnumerable<IEnumerable<Vector2>> GetContourPoints()
        {
            for(var x = -CellAmountOnAbscissaAxe /2; x <= CellAmountOnAbscissaAxe/2; x++)
            {
                yield return GetOrdinateLine(x);
            }

            for (var y = -CellAmountOnOrdinateAxe / 2; y <= CellAmountOnOrdinateAxe / 2; y++)
            {
                yield return GetAbscissaLine(y);
            }
        }
    }
}
