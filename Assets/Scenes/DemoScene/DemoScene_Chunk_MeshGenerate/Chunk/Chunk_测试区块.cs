using DemoScene_ChunkService_DynamicChunk;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    
    public class Chunk_测试区块: MC_Base_Chunk
    {
        public Chunk_测试区块(ChunkInitData initData, IChunkRenderer renderer) : base(initData, renderer) { }

        public override void CalculateData()
        {
            for (int i = 0; i < chunkMicroData.Count; i++)
                chunkMicroData.SetVoxel(i, type: 1);
        }

        public override void CalculateGrid()
        {
            List<Vector3> vertices = new List<Vector3>();

            //Triangles
            int subMeshCount = 0; 
            List<int>[] triangles = new List<int>[subMeshCount];
            for (int i = 0; i < subMeshCount; i++)
                triangles[i] = new List<int>();

            List<Vector2> uvs = new List<Vector2>();

            // 遍历每一个元素
            for (int thisLinearCoord = 0; thisLinearCoord < chunkMicroData.Count; thisLinearCoord++)
            {
                // 遍历每一个方向
                for (int face = 0; face < 6; face++)
                {
                    Orientation orientation = (Orientation)face;
                    Vector3Int thisCoord = LinearToLogic(thisLinearCoord);
                    Vector3Int targetCoord = thisCoord + MC_Static_区块数据.面朝方向[face];

                    // 跳过绘制-当前方向上有固体
                    if (isSolid(targetCoord))
                        continue;

                    // 绘制面
                    MC_Static_区块数据.GetQuad(orientation, out List<Vector3> quadVertices, out List<int> quadTriangles, out List<Vector2> quadUvs);

                    vertices.AddRange(quadVertices);
                    triangles[0].AddRange(quadTriangles);
                    uvs.AddRange(quadUvs);

                }
                Vector3 chunkWorldPos = MC_Util_Math.LogicToWorld(chunkMacroData.chunkSize, chunkMacroData.chunkLogicPos);
                chunkRenderData.Set(vertices.ToArray(), uvs.ToArray(), triangles, chunkWorldPos);

            }

        }
    }
}