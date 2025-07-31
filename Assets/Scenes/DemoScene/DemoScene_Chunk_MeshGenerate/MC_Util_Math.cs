using DemoScene_ChunkService_DynamicChunk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    public static class MC_Util_Math
    {
        /// <summary>
        /// 逻辑坐标映射到世界坐标
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <param name="logicPos"></param>
        /// <returns></returns>
        public static Vector3 LogicToWorld(Vector3Int chunkSize, Vector3Int logicPos)
        {
            return new Vector3(logicPos.x * chunkSize.x, logicPos.y * chunkSize.y, logicPos.z * chunkSize.z);
        }

        /// <summary>
        /// 世界坐标映射到逻辑坐标
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <param name="relaPos"></param>
        /// <returns></returns>
        public static Vector3Int WorldToLogic(Vector3Int chunkSize, Vector3 relaPos)
        {
            return new Vector3Int((int)relaPos.x / chunkSize.x, (int)relaPos.y / chunkSize.y, (int)relaPos.z / chunkSize.z);
        }
    }
}
