using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    /// <summary>
    /// ���鷽�򹤾���
    /// </summary>
    public static class MC_Define_Config_Orientation
    {
        /// <summary>
        /// �������������ö�٣�
        /// </summary>
        public enum Enum_Orientation : byte
        {
            North = 0, //Front
            South = 1, //Back
            West = 2,  //Left
            East = 3,  //Right
            Top = 4,   //Top
            Bottom = 5 //Bottom
        }

        /// <summary>
        /// �볯��ö��һһ��Ӧ��������������
        /// </summary>
        public static readonly Vector3Int[] Vec_Orientation = new Vector3Int[6]
        {
            new Vector3Int( 0,  0,  1), // North
            new Vector3Int( 0,  0, -1), // South
            new Vector3Int(-1,  0,  0), // West
            new Vector3Int( 1,  0,  0), // East
            new Vector3Int( 0,  1,  0), // Top
            new Vector3Int( 0, -1,  0), // Bottom
        };

        /// <summary>
        /// ��ȡָ�������Ӧ�ķ�������
        /// </summary>
        public static Vector3Int GetDirection(Enum_Orientation orientation)
        {
            return Vec_Orientation[(int)orientation];
        }
    }
}
