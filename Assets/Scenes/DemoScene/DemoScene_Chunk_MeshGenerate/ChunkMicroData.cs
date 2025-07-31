using System;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    /// <summary>
    /// 区块中的体素微观数据集合
    /// </summary>
    [Serializable]
    public class ChunkMicroData
    {
        [SerializeField]
        private Voxel[] voxelMap;

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
            voxelMap = new Voxel[size];
            for (int i = 0; i < size; i++)
            {
                voxelMap[i] = Voxel.Default; // 使用结构体的默认值定义
            }
        }

        /// <summary>
        /// 设置指定体素的值（可部分设置）
        /// </summary>
        public void SetVoxel(int index, byte? type = null, Orientation? orientation = null, byte? light = null)
        {
            if (IsValidIndex(index))
            {
                ref Voxel v = ref voxelMap[index];
                if (type.HasValue) v.Type = type.Value;
                if (orientation.HasValue) v.Orientation = orientation.Value;
                if (light.HasValue) v.Light = light.Value;
            }
            else
            {
                Debug.LogWarning($"SetVoxel index out of range: {index}");
            }
        }

        /// <summary>
        /// 获取指定体素
        /// </summary>
        public Voxel GetVoxel(int index)
        {
            if (IsValidIndex(index))
                return voxelMap[index];

            Debug.LogWarning($"GetVoxel index out of range: {index}");
            return Voxel.Default;
        }

        /// <summary>
        /// 检查索引是否有效
        /// </summary>
        private bool IsValidIndex(int index) => voxelMap != null && index >= 0 && index < voxelMap.Length;
    }

    /// <summary>
    /// 单个体素（块）数据
    /// </summary>
    [Serializable]
    public struct Voxel
    {
        public byte Type;
        public Orientation Orientation;
        public byte Light;

        public static Voxel Default => new Voxel
        {
            Type = 0,
            Orientation = Orientation.North,
            Light = 7
        };

        public bool IsSolid => Type != 0;
    }

}
