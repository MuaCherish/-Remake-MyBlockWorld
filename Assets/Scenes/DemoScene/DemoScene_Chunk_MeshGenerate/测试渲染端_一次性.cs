using DemoScene_ChunkService_DynamicChunk;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    
    public class 测试渲染端_一次性 : MonoBehaviour, IChunkRenderer
    {
        public Camera cam;
        private Plane[] 视锥体平面;
        public List<MC_Base_Chunk> AllChunks = new List<MC_Base_Chunk>();
        public int count;

        public void Update()
        {
            视锥体平面 = GeometryUtility.CalculateFrustumPlanes(cam);
            RenderChunk();
            count = AllChunks.Count;
        }

        public void PullData(MC_Base_Chunk chunk)
        {
            AllChunks.Add(chunk);

            Debug.Log($"区块坐标：{chunk.chunkMacroData.chunkLogicPos}" +
                      $"区块尺寸：{chunk.chunkMacroData.chunkSize}" +
                      $"体素数量：{chunk.chunkMicroData.Count}" +
                      $"第一个体素类型：{chunk.chunkMicroData.GetVoxel(0).Type}" +
                      $"verticesCount：{chunk.chunkRenderData.ChunkMesh.vertices.Length}"
                      );

        }

        public void RenderChunk()
        {
            if (AllChunks.Count == 0)
                return;

            foreach (var chunk in AllChunks)
            {
                if (chunk == null || !chunk.chunkRenderData.IsValid)
                    continue;

                Vector3 size = chunk.chunkMacroData.chunkSize;
                Vector3 chunkWorldPos = MC_Util_Math.LogicToWorld(chunk.chunkMacroData.chunkSize, chunk.chunkMacroData.chunkLogicPos);
                Bounds bounds = new Bounds(chunkWorldPos + size * 0.5f, size);

                for (int i = 0; i < chunk.chunkRenderData.ChunkMesh.subMeshCount; i++)
                {
                    Material mat = (i < chunk.chunkRenderData.ChunkMaterials.Length) ? chunk.chunkRenderData.ChunkMaterials[i] : chunk.chunkRenderData.ChunkMaterials[0];
                    Graphics.DrawMesh(
                        chunk.chunkRenderData.ChunkMesh,
                        chunk.chunkRenderData.Matrix,
                        mat,
                        0,
                        cam,
                        i // 指定绘制第i个子网格
                    );
                }

            }
        }
    }
}
