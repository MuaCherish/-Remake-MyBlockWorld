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

            //直接渲染chunk
            Test_RenderChunk(chunk);
        }

        public void Test_RenderChunk(MC_Base_Chunk chunk)
        {
            if (chunk.chunkRenderData == null || !chunk.chunkRenderData.IsValid)
            {
                Debug.LogWarning("Chunk 渲染数据无效，跳过 GameObject 创建");
                return;
            }

            // 创建新的 GameObject
            GameObject chunkObj = new GameObject("Chunk_" + chunk.chunkMacroData.chunkLogicPos.ToString());

            // 设置位置（可选，已经在 Matrix 里设置了）
            chunkObj.transform.position = chunk.chunkRenderData.Matrix.GetColumn(3); // 取平移向量

            // 添加 MeshFilter 和 MeshRenderer
            var mf = chunkObj.AddComponent<MeshFilter>();
            var mr = chunkObj.AddComponent<MeshRenderer>();

            // 设置 Mesh 和材质
            mf.sharedMesh = chunk.chunkRenderData.ChunkMesh;
            mr.materials = chunk.chunkRenderData.ChunkMaterials;

            // 可选：挂回 chunk 本身，便于后续更新（如 AddComponent<MonoChunkHolder>().Init(chunk);）
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
                Vector3 chunkWorldPos = MC_Util_Math.Macro_LogicToWorld(chunk.chunkMacroData.chunkSize, chunk.chunkMacroData.chunkLogicPos);
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
