using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    public class ChunkInitData
    {
        public Vector3Int chunkLogicPos;
        public Vector3Int chunkSize;
        public Material[] chunkMaterials;

        public ChunkInitData(Vector3Int logicPos, Vector3Int size, Material[] material)
        {
            chunkLogicPos = logicPos;
            chunkSize = size;
            chunkMaterials = material;
        }
    }
}