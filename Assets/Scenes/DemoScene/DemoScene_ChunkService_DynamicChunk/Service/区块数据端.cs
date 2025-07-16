using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    public class 区块数据端 : MonoBehaviour
    {
        //数据
        public 优先级字典 AllChunks;
        public 区块全局设置 区块全局数据;

        //引用
        public Material Mat_Chunk;
        public Camera Player_Camera;
        public Transform Player_Transform;

        [ReadOnly] public int ChunksCount;

        private void Awake()
        {
            AllChunks = new 优先级字典(Player_Transform, this);
        }

        private void Update()
        {
            ChunksCount = AllChunks.Count;
        }
    }


}





