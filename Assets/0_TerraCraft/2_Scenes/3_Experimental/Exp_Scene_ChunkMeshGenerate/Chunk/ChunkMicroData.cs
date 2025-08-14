using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    /// <summary>
    /// 区块中的体素微观数据集合
    /// </summary>
    [Serializable]
    public class ChunkMicroData
    {
        [SerializeField]
        private MC_Define_Class_VoxelState[] voxelMap;

        /// <summary>
        /// 当前区块的体素数量
        /// </summary>
        public int Count => voxelMap?.Length ?? 0;

        /// <summary>
        /// 初始化体素数据（设置为指定大小）
        /// </summary>
        /// <param name="size">体素总数量（一般为 区块宽×高×深）</param>
        public void Initialize(int size)
        {
            voxelMap = new MC_Define_Class_VoxelState[size];
            for (int i = 0; i < size; i++)
            {
                voxelMap[i] = MC_Define_Class_VoxelState.Default; // 使用结构体的默认值定义
            }
        }

        /// <summary>
        /// 设置指定体素的值（可部分设置）
        /// </summary>
        public void SetVoxel(
            int index,
            byte? type = null,
            MC_Define_Config_Orientation.Enum_Orientation? orientation = null,
            byte[] lightArray = null,
            int? lightDir = null,
            byte? lightValue = null)
        {
            if (!IsValidIndex(index))
            {
                //Debug.LogWarning($"SetVoxel index out of range: {index}");
                return;
            }

            ref MC_Define_Class_VoxelState v = ref voxelMap[index];

            if (type.HasValue)
                v.Type = type.Value;

            if (orientation.HasValue)
                v.Orientation = orientation.Value;

            if (lightArray != null && lightArray.Length == 6)
            {
                Array.Copy(lightArray, v.Light, 6); // 整组设置
            }
            else if (lightDir.HasValue && lightValue.HasValue)
            {
                if (lightDir.Value >= 0 && lightDir.Value < 6)
                    v.Light[lightDir.Value] = lightValue.Value; // 单面设置
                else
                    Debug.LogWarning($"Invalid light direction: {lightDir.Value}");
            }
        }


        /// <summary>
        /// 获取指定体素
        /// </summary>
        public MC_Define_Class_VoxelState GetVoxel(int index)
        {
            if (IsValidIndex(index))
                return voxelMap[index];

            //Debug.LogWarning($"GetVoxel index out of range: {index}");
            return MC_Define_Class_VoxelState.Default;
        }

        /// <summary>
        /// 检查索引是否有效
        /// </summary>
        private bool IsValidIndex(int index) => voxelMap != null && index >= 0 && index < voxelMap.Length;

        /// <summary>
        /// 判断指定逻辑位置是否越界
        /// </summary>
        /// <param name="chunkSize">区块尺寸（宽、高、深）</param>
        /// <param name="logicPos">体素逻辑坐标</param>
        /// <returns>是否越界</returns>
        public bool IsOutOfIndex(Vector3Int chunkSize, Vector3Int logicPos)
        {
            return logicPos.x < 0 || logicPos.x >= chunkSize.x ||
                   logicPos.y < 0 || logicPos.y >= chunkSize.y ||
                   logicPos.z < 0 || logicPos.z >= chunkSize.z;
        }

        /// <summary>
        /// 工具方法：判断是否是固体
        /// </summary>
        public bool isSolid(Vector3Int chunkSize, Vector3Int thisRelaPos)
        {
            //提前返回-数组越界一律绘制（后面可以尝试检查目标区块的相应位置）
            if (IsOutOfIndex(chunkSize, thisRelaPos))
                return false;

            //是空气则绘制
            if (GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, thisRelaPos)).Type == MC_Define_Config_VoxelId.Air ||
                GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, thisRelaPos)).Type == MC_Define_Config_VoxelId.Water
                )
                return false;
            else
                return true;
        }


        /// <summary>
        /// 工具方法：判断是否需要绘制
        /// True:绘制
        /// False:不绘制
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool isNeedDrawQuad(Vector3Int chunkSize, Vector3Int thisRelaCoord, Vector3Int targetRelaCoord)
        {

            byte thisType = GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, thisRelaCoord)).Type;
            byte targetType = GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, targetRelaCoord)).Type;

            //提前绘制-下标出界
            if (thisType != MC_Define_Config_VoxelId.Air && IsOutOfIndex(chunkSize, targetRelaCoord))
                return true;

            //提前绘制-如果是透明方块
            if (isSolid(chunkSize, thisRelaCoord) && !isSolid(chunkSize, targetRelaCoord))
                return true;

            return false;
        }

    }


}
