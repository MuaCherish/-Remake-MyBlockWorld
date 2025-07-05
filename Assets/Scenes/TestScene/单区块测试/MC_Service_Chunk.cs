using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC_Test.RenderOneChunk
{
    public class MC_Service_Chunk : MonoBehaviour
    {
        public SO_Test_ChunkSetting WorldSetting;

        void Start()
        {
            ChunkInfo _chunkData = new ChunkInfo();
            _chunkData.name = Vector3.zero.ToString();
            _chunkData.chunkSize = WorldSetting.ChunkSize;

            Chunk chunk = new Chunk(_chunkData);
        }

    }
}


