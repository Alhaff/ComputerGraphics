using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public struct MyMatrix3x3
    {
        #region Variables
        public readonly float[,] matrix = new float[3,3];
        #endregion

        #region Constructors
        public MyMatrix3x3(float[,] m)
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    matrix[i, j] = m[i, j];
                }
            }
        }
        public MyMatrix3x3(params float[] values)
        {
            if (values.Length >= 9)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        matrix[i, j] = values[3 * i + j];
                    }
                }
            }
        }
        #endregion

        #region Operators
        public float this[int i,int j]
        {
            get
            {
                if(i >= 0 && i < matrix.GetLength(0))
                {
                    if(j >= 0 && j < matrix.GetLength(1))
                    {
                        return matrix[i, j];
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }
        public static Vector3 operator*(Vector3 vect, MyMatrix3x3 matrix)
        {
            float x = vect.X * matrix[0,0] + vect.Y * matrix[1,0] + vect.Z * matrix[2,0];
            float y = vect.X * matrix[0, 1] + vect.Y * matrix[1, 1] + vect.Z * matrix[2, 1];
            float z = vect.X * matrix[0, 2] + vect.Y * matrix[1, 2] + vect.Z * matrix[2, 2];
            x /= z;
            y /= z;
            z /= z;
            return new Vector3(x, y, z);
        }
        #endregion
    }
}
