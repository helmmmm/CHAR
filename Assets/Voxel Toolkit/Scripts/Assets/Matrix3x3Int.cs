using UnityEngine;

namespace VoxelToolkit
{
    /// <summary>
    /// The structure that represents integer matrix
    /// </summary>
    [System.Serializable]
    public struct Matrix3x3Int
    {
        public int E00, E10, E20;
        public int E01, E11, E21;
        public int E02, E12, E22;
        
        /// <summary>
        /// Identity matrix
        /// </summary>
        public static readonly Matrix3x3Int Identity = new Matrix3x3Int(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1);
        
        public Matrix3x3Int(int e00, int e10, int e20, int e01, int e11, int e21, int e02, int e12, int e22)
        {
            E00 = e00;
            E10 = e10;
            E20 = e20;
            E01 = e01;
            E11 = e11;
            E21 = e21;
            E02 = e02;
            E12 = e12;
            E22 = e22;
        }

        /// <summary>
        /// Multiplication operation between Vector3Int and Matrix3x3Int
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3Int operator *(Matrix3x3Int matrix, Vector3Int vector)
        {
            return new Vector3Int(
                vector.x * matrix.E00 + vector.y * matrix.E10 + vector.z * matrix.E20,
                vector.x * matrix.E01 + vector.y * matrix.E11 + vector.z * matrix.E21,
                vector.x * matrix.E02 + vector.y * matrix.E12 + vector.z * matrix.E22);
        }

        /// <summary>
        /// Converts #VoxelToolkit.Matrix3x3Int to an ordinary Matrix4x4
        /// </summary>
        /// <param name="inMatrix">The matrix to be converted</param>
        /// <returns>Converted to Matrix4x4 original matrix</returns>
        public static implicit operator Matrix4x4(Matrix3x3Int inMatrix)
        {
            return new Matrix4x4(
                new Vector4(inMatrix.E00, inMatrix.E10, inMatrix.E20, 0),
                new Vector4(inMatrix.E01, inMatrix.E11, inMatrix.E21, 0),
                new Vector4(inMatrix.E02, inMatrix.E12, inMatrix.E22, 0),
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        }
    }
}