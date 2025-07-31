using DemoScene_Chunk_MeshGenerate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    public interface IChunkRenderer
    {
        /// <summary>
        /// ½ÓÊÕÇø¿é
        /// </summary>
        /// <param name="chunk"></param>
        public void PullData(MC_Base_Chunk chunk);
    }
}
