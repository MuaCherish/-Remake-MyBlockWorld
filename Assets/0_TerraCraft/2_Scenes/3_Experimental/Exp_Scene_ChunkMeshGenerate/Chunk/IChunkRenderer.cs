using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
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
