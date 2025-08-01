using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings.SplashScreen;

namespace DemoScene_Chunk_MeshGenerate
{
    /// <summary>
    /// 所有方块类型的配置数据库（ScriptableObject）
    /// </summary>
    [CreateAssetMenu(fileName = "VoxelDatabase", menuName = "ScriptableObjects/Voxel Database")]
    public class VoxelDatabase : ScriptableObject
    {
        public List<MC_Define_VoxelDefinition> voxelList = new List<MC_Define_VoxelDefinition>();
    }

}
