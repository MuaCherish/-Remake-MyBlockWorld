using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    public class Y小于等于零规则 : IChunkSpawnRule
    {
        public bool IsValid(Vector3Int logicPos)
        {
            return logicPos.y <= 0;
        }
    }

    public class 加载端测试器 : MonoBehaviour
    {
        public 区块加载端 加载端;

        private void Awake()
        {
            加载端.生成规则 = new Y小于等于零规则();
        }


    }

}

