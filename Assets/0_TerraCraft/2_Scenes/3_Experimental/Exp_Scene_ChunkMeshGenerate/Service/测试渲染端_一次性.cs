using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Tc.Exp.ChunkMeshGenerate
{
    public class 测试渲染端_一次性 : MonoBehaviour, IChunkRenderer
    {
        public Camera cam;
        private Plane[] 视锥体平面;
        public List<MC_Base_Chunk> AllChunks = new List<MC_Base_Chunk>();


        public void Update()
        {
            视锥体平面 = GeometryUtility.CalculateFrustumPlanes(cam);
            RenderChunk();
        }

        public void PullData(MC_Base_Chunk chunk)
        {
            AllChunks.Add(chunk);

            // 如果你要立即渲染：
            Test_RenderChunk(chunk);
        }

        public void Test_RenderChunk(MC_Base_Chunk chunk)
        {
            if (chunk.chunkRenderData == null || !chunk.chunkRenderData.IsValid)
            {
                UnityEngine.Debug.LogWarning("Chunk 渲染数据无效，跳过 GameObject 创建");
                return;
            }

            GameObject chunkObj = new GameObject("Chunk_" + chunk.chunkMacroData.chunkLogicPos.ToString());
            chunkObj.transform.position = chunk.chunkRenderData.Matrix.GetColumn(3);

            var mf = chunkObj.AddComponent<MeshFilter>();
            var mr = chunkObj.AddComponent<MeshRenderer>();

            mf.sharedMesh = chunk.chunkRenderData.ChunkMesh;
            mr.materials = chunk.chunkRenderData.ChunkMaterials;
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
                        i
                    );
                }
            }
        }
    }
}
