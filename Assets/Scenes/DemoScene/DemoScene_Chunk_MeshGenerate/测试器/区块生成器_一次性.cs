using DemoScene_ChunkService_DynamicChunk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    public class 区块生成器_一次性 : MonoBehaviour
    {
        public Vector3Int LogicPos;
        public Vector3Int Size; 
        public Material[] Mat_Chunk;
        public 测试渲染端_一次性 渲染端;

        void Start()
        {
            ChunkInitData initData = new ChunkInitData(LogicPos, Size, Mat_Chunk);
            Chunk_测试区块 chunk = new Chunk_测试区块(initData, 渲染端);
        }

 
    }
}


