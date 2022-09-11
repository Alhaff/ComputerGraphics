using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CooridnateGrid.DrawnObjects
{
    /// <summary>
    /// Інтерфейс, що мають успадковувати об'єкти, які мають бути намальовані
    /// </summary>
    public interface IDrawnObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Повертає перелік точок контуру рисунку. Так як об'єкт може мати не суцільний контур 
        /// повертає його у вигляді переліку точок які можна з'єднати нерозривною лінією.
        /// </summary>
        /// <returns>type - IEnumerable<IEnumerable<Vector2>></returns>
        public IEnumerable<IEnumerable<Vector2>> GetContourPoints();
        /// <summary>
        /// Колір лінії контуру об'єкта
        /// </summary>
        public Color MyColor { get; set; }
        /// <summary>
        /// Функція, що буду застосована, до всіх точок об'єкту
        /// </summary>
        public Func<Vector2, Vector2> TransformFunctions { get; set; }
    }
}
