using MC_Test.RenderOneChunk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC_Test.RenderOneChunk
{
    public class ChunkInfo
    {
        public string name;
        public Vector3 chunkSize;
    }

    public class Chunk
    {

        public ChunkInfo myChunkData;

        public Chunk(ChunkInfo chunkData)
        {
            //提前返回-传入区块数据有问题
            if (chunkData == null)
            {
                Debug.Log("传入的区块数据有问题");
                return;
            }

            //数据初始化
            myChunkData = chunkData;



            //Finish
            Debug.Log($"Chunk{chunkData.name} Created");
        }
    }

}


//namespace MC
//{
//    public class A
//    {
//        public A()
//        {
//            Debug.Log($"{MC_Static_Chunk.ChunkSize}");
//        }
//    }
//}