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
        private Color _myColor;
        /// <summary>
        /// Повертає перелік точок контуру рисунку. Так як об'єкт може мати не суцільний контур 
        /// повертає його у вигляді переліку точок які можна з'єднати нерозривною лінією.
        /// </summary>
        /// <returns>type - IEnumerable<IEnumerable<Vector3>></returns>
        public abstract IEnumerable<IEnumerable<Vector3>> GetContourPoints();
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
        /// <summary>
        /// Функція, що буду застосована, до всіх точок об'єкту
        /// </summary>
        public Func<Vector3, Vector3> TransformMe { get; set; } = v => v;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
