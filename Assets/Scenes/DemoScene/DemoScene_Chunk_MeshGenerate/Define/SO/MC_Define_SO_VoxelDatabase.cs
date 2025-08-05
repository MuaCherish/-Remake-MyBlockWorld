using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings.SplashScreen;

namespace DemoScene_Chunk_MeshGenerate
{
    /// <summary>
    /// 所有方块类型的配置数据库（ScriptableObject）
    /// </summary>
    [CreateAssetMenu(fileName = "VoxelDatabase", menuName = "ScriptableObjects/Voxel Database")]
    public class MC_Define_SO_VoxelDatabase : ScriptableObject
    {
        public List<MC_Define_Class_VoxelDefinition> voxelList = new List<MC_Define_Class_VoxelDefinition>();
    }

}
