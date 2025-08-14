using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UI/面板配置表")]
    public class UI配置表 : ScriptableObject
    {
        [System.Serializable]
        public class UI面板配置项
        {
            public string 名称;
            public GameObject 预制体; // 或 string 资源路径
        }

        public List<UI面板配置项> 所有UI面板;
    }

}

