using System.Collections;
using UnityEngine;

namespace Tc.Exp.ChunkService
{

    /// <summary>
    /// 具体规则实现：只允许 Y 小于等于 0 的区块生成
    /// </summary>
    public class Y小于等于零规则 : IChunk_Updater_Rule
    {
        public bool IsValid(Vector3Int logicPos)
        {
            return logicPos.y <= 0;
        }
    }

    /// <summary>
    /// 测试器脚本，挂载于测试场景
    /// </summary>
    public class 加载端测试器 : MonoBehaviour
    {
        public 区块加载端 加载端;

        public bool 只加载一次 = false;
        public bool 启用Y小于等于零规则 = true;

        private void Awake()
        {
            if (启用Y小于等于零规则)
            {
                加载端.生成规则 = new Y小于等于零规则();
            }
            else
            {
                加载端.生成规则 = null;
            }



            if (只加载一次)
            {
                加载端.SetAutoUpdateValue(false);
                StartCoroutine(LoadChunkOnce());
            }
        }

        IEnumerator LoadChunkOnce()
        {
            // 等待预热完成
            yield return new WaitUntil(() => 加载端.CheckWarmUp());

            // 加载一次
            加载端.Service_DynamicChunkLoad();

            // 协程自然退出
        }

    }
}
