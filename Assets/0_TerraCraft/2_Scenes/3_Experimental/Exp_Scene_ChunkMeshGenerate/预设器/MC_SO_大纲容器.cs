using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    [Serializable]
    public class 大纲容器
    {
        public string memo; //备注
        public string MyObjName;       // 创建的 GameObject 名字
        public string CsObjParent;     // 父路径，如 "服务端/Chunk"
        public string CsName;          // 脚本完整类型名，如 "DemoScene_Chunk_MeshGenerate.我的脚本"
    }

    [CreateAssetMenu(fileName = "大纲预设", menuName = "ScriptableObjects/大纲容器", order = 1)]
    public class MC_SO_大纲容器 : ScriptableObject
    {
        public List<大纲容器> 预设列表;
    }
}

