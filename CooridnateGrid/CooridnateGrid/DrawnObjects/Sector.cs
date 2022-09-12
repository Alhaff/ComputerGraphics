using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CooridnateGrid.DrawnObjects
{
    /// <summary>
    /// <para>Визначає клас сектора, під сектор будемо розуміти коло, що має розрив у двох токах.</para>
    /// <para>За замовчування визначається довжина лінії розриву,
    /// де стартова точка відповідає точці на колі з кутом fi = 0 
    ///</para>
    /// </summary>
    public class Sector : IDrawnObject
    {
        //variables
        #region
        private Vector2 _sectorEnd;
        private double _lenBetweenTwoBreakPoints;
        public readonly bool IsSegmentExistBetweenBreakPoints;
        private readonly double LenToDRelation;
        private Vector2 _center;
        private double _r;
        private Func<Vector2, Vector2> _transformFunctions;
        private Color _myColor;
       
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        //Propreties
        #region
        public Vector2 Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Center");
            }
        }
        public double R
        {
            get { return _r; }
            set
            {
                _r = value;
                if(_lenBetweenTwoBreakPoints / (2 * _r) != LenToDRelation)
                    LenBetweenTwoBreakPoints = LenToDRelation * (2 *_r);
                OnPropertyChanged("R");
            }
        }
        /// <summary>
        /// Відмтань між початковую та кінцевою координатою
        /// </summary>
        public double LenBetweenTwoBreakPoints
        {
            get { return _lenBetweenTwoBreakPoints; }
            set
            {
                _lenBetweenTwoBreakPoints = value;
                if (_lenBetweenTwoBreakPoints / (2 * _r) != LenToDRelation)
                    R = _lenBetweenTwoBreakPoints /  (2 * LenToDRelation);
                OnPropertyChanged("LenBetweenTwoBreakPoints");
            }
        }
        public Func<Vector2, Vector2> TransformFunctions
        {
            get => _transformFunctions;
            set
            {
                _transformFunctions = value;
                OnPropertyChanged("TransformFunctions");
            }
        }
        /// <summary>
        /// Початкова координата розрива
        /// <para>за замовчуваням знаходиться на колі з кутом fi = 0</para>
        /// </summary>
        public Vector2 SectorStart { get => Center + new Vector2((float)R, 0); }
        /// <summary>
        /// Кінцева координата
        /// <para>Визначається як точка, що лежить на колі за додатнім напрямком кута fi та знаходиться на відстані L заданої при створені</para>  
        /// </summary>
        public Vector2 SectorEnd { get => SectorStart.Rotate(2 * Math.Asin(LenToDRelation)); }
        public Color MyColor
        {
            get { return _myColor; }
            set
            {
                _myColor = value;
                OnPropertyChanged("MyColor");
            }
        }
        #endregion

        /// <summary>
        /// Створює обєкт сектора
        /// </summary>
        /// <param name="center">Центр сектора</param>
        /// <param name="r">Радіус</param>
        /// <param name="len">Довжина лінії між точками розриву</param>
        /// <param name="isBetweenPoints"> Малювати область між точками розриву, чи поза нею</param>
        /// <param name="color">Колір, яким буде намальовано контур</param>
        /// <param name="rotateFunc">Функція що буде застосована до всіх точок сектора при його друкуванні, у даному випадку обертання сектора</param>
        public Sector(Vector2 center, double r, double len, bool isBetweenPoints, Color color, Func<Vector2, Vector2> rotateFunc)
        {
            LenToDRelation = len / (2 * r);
            LenBetweenTwoBreakPoints = len;
            Center = center;
            MyColor = color;
            TransformFunctions = rotateFunc;
            IsSegmentExistBetweenBreakPoints = isBetweenPoints;
            
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private IEnumerable<Vector2> GetSectorPoints()
        {
            double startAngle = IsSegmentExistBetweenBreakPoints ? 0 : SectorEnd.Angle();
            double endAngle = IsSegmentExistBetweenBreakPoints ? SectorEnd.Angle() : 2* Math.PI;
            for (double t = startAngle; t <= endAngle; t += Math.PI / 36)
            {
                var x = R * Math.Cos(t) + Center.X;
                var y = R * Math.Sin(t) + Center.Y;
                yield return new Vector2((float)x, (float)y);
            }
        }
        public IEnumerable<IEnumerable<Vector2>> GetContourPoints()
        {
            yield return GetSectorPoints();
        }

       
    }
}
