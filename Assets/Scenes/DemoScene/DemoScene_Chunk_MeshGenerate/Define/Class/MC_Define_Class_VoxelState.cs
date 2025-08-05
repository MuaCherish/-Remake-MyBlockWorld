using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{

    /// <summary>
    /// 单个体素（块）数据
    /// </summary>
    [Serializable]
    public struct MC_Define_Class_VoxelState
    {
        public byte Type;
        public MC_Define_Config_Orientation.Enum_Orientation Orientation;
        public byte[] Light;

        public static MC_Define_Class_VoxelState Default => new MC_Define_Class_VoxelState
        {
            Type = 0,
            Orientation = MC_Define_Config_Orientation.Enum_Orientation.North,
            Light = new byte[6] { 0,0,0,0,0,0 },
        };
    }
}