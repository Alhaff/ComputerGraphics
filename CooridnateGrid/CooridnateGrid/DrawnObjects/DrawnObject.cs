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

namespace CooridnateGrid.DrawnObjects
{
    /// <summary>
    /// Абстрактний класс, що мають успадковувати об'єкти, які мають бути намальовані
    /// </summary>
    public abstract class DrawnObject : INotifyPropertyChanged
    {
        #region Variables
        private Color _myColor;
        #endregion

        #region Propreties
        /// <summary>
        /// Колір лінії контуру об'єкта
        /// </summary>
        public Color MyColor
        {
            get { return _myColor; }
            set
            {
                _myColor = value;
                OnPropertyChanged("MyColor");
            }
        }
        public bool IsPlaneTransfromMe { get; set; } = true;
        /// <summary>
        /// Функція, що буде застосована, до всіх точок об'єкту, які поврене метод ContourPoints()
        /// </summary>
        public Func<Vector3, Vector3> TransformMe { get; set; } = v => v;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Methods
        /// <summary>
        /// Повертає перелік точок контуру рисунку. Так як об'єкт може мати не суцільний контур 
        /// повертає його у вигляді переліку точок які можна з'єднати нерозривною лінією.
        /// </summary>
        /// <returns>type - IEnumerable<IEnumerable<Vector3>></returns>
        protected abstract IEnumerable<IEnumerable<Vector3>> ContourPoints();

        public IEnumerable<IEnumerable<Vector3>> GetContourPoints()
        {
            foreach (var countour in ContourPoints())
                yield return countour.Select(point => TransformMe(point));
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
