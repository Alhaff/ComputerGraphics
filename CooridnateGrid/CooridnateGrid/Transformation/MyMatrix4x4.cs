using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.Transformation
{
    public struct MyMatrix4x4
    {
        #region Variables
        public readonly float[,] matrix = new float[4, 4];
        #endregion

        #region Constructors
        public MyMatrix4x4(float[,] m)
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix[i, j] = m[i, j];
                }
            }
        }
        public MyMatrix4x4(params float[] values)
        {
            if (values.Length >= 16)
            {
                for (var i = 0; i < 4; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        matrix[i, j] = values[4 * i + j];
                    }
                }
            }
        }
        #endregion

        #region Operators
        public float this[int i, int j]
        {
            get
            {
                if (i >= 0 && i < matrix.GetLength(0))
                {
                    if (j >= 0 && j < matrix.GetLength(1))
                    {
                        return matrix[i, j];
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }
        public static Vector3 operator *(Vector3 vect, MyMatrix4x4 Matrix)
        {
            var extVect = new Vector4(vect.X, vect.Y, vect.Z, 1);
            float x = extVect.X * Matrix[0, 0] + extVect.Y * Matrix[1, 0] 
                    + extVect.Z * Matrix[2, 0] + extVect.W * Matrix[3,0];
            float y = extVect.X * Matrix[0, 1] + extVect.Y * Matrix[1, 1] 
                    + extVect.Z * Matrix[2, 1] + extVect.W * Matrix[3, 1];
            float z = extVect.X * Matrix[0, 2] + extVect.Y * Matrix[1, 2] 
                    + extVect.Z * Matrix[2, 2] + extVect.W * Matrix[3, 2];
            float w = extVect.X * Matrix[0, 3] + extVect.Y * Matrix[1, 3]
                    + extVect.Z * Matrix[2, 3] + extVect.W * Matrix[3, 3];
            x /= w;
            y /= w;
            z /= w;
            w /= w;
            return new Vector3(x, y, z);
        }
        #endregion
    }
}

