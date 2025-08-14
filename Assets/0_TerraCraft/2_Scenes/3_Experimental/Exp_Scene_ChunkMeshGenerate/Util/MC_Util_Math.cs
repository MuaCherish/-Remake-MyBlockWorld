using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    public static class MC_Util_Math
    {
        /// <summary>
        /// 宏观工具函数：逻辑坐标映射到世界坐标
        /// </summary>
        public static Vector3 Macro_LogicToWorld(Vector3Int chunkSize, Vector3Int logicPos)
        {
            return new Vector3(logicPos.x * chunkSize.x, logicPos.y * chunkSize.y, logicPos.z * chunkSize.z);
        }

        /// <summary>
        /// 宏观工具函数：世界坐标映射到逻辑坐标
        /// </summary>
        public static Vector3Int Macro_WorldToLogic(Vector3Int chunkSize, Vector3 relaPos)
        {
            return new Vector3Int((int)relaPos.x / chunkSize.x, (int)relaPos.y / chunkSize.y, (int)relaPos.z / chunkSize.z);
        }

        /// <summary>
        ///  微观工具函数：三维变一维
        /// </summary>
        public static int Micro_RelaToLinear(Vector3Int chunkSize, Vector3Int coord)
        {
            if (coord.x < 0 || coord.x >= chunkSize.x ||
                coord.y < 0 || coord.y >= chunkSize.y ||
                coord.z < 0 || coord.z >= chunkSize.z)
            {
                //Debug.LogError($"索引越界: ({coord.x},{coord.y},{coord.z}) 超出范围 {chunkSize}");
                return -1;
            }
            return coord.x + chunkSize.x * (coord.y + chunkSize.y * coord.z);
        }

        /// <summary>
        /// 微观工具函数：一维变三维
        /// </summary>
        public static Vector3Int Micro_LinearToRela(Vector3Int chunkSize, int index) 
        {
            if (index < 0 || index >= chunkSize.x * chunkSize.y * chunkSize.z)
            {
                Debug.LogError($"索引越界: index={index} 超出范围 {chunkSize.x * chunkSize.y * chunkSize.z}");
                return Vector3Int.zero;
            }
            int x = index % chunkSize.x;
            int y = (index / chunkSize.x) % chunkSize.y;
            int z = index / (chunkSize.x * chunkSize.y);
            return new Vector3Int(x, y, z);
        }

    }
}
