using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    public class 区块生成器_一次性 : MonoBehaviour
    {
        public ChunkInitData initData;
        public 测试渲染端_一次性 渲染端;

        void Start()
        {
            Chunk_测试区块 _chunk = new Chunk_测试区块(initData, 渲染端);
        }

    }
}


