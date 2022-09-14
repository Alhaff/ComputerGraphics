using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
    public interface IDrawingSelf
    {
        public void Draw(CoordinatePlane.MyPlane pl);
    }
}
