using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public abstract class Transformation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public abstract Func<Vector3,Vector3> Transform { get; }

        public static  implicit operator Func<Vector3, Vector3>(Transformation tr) => tr.Transform;

    }
}
