using DemoScene_ChunkService_DynamicChunk;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    public class Chunk_测试区块 : MC_Base_Chunk
    {
        public Chunk_测试区块(ChunkInitData initData, IChunkRenderer renderer) : base(initData, renderer) { }

        public override void CalculateData()
        {
            for (int i = 0; i < chunkMicroData.Count; i++)
                chunkMicroData.SetVoxel(i, type: MC_Define_VoxelId.Stone);

            //设置一个测试光源
            int index = MC_Util_Math.Micro_RelaToLinear(chunkMacroData.chunkSize, new Vector3Int(0, 0, 0));
            chunkMicroData.SetVoxel(index, lightDir: (int)MC_Define_Orientation.Enum_Orientation.Top, lightValue: 15);

        }

        public override void CalculateGrid()
        {
            int verticesIndexHead = 0;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Color> colors = new List<Color>(); // ⬅ 顶点颜色

            int subMeshCount = 1;
            List<int>[] triangles = new List<int>[subMeshCount];
            for (int i = 0; i < subMeshCount; i++)
                triangles[i] = new List<int>();

            for (int thisLinearCoord = 0; thisLinearCoord < chunkMicroData.Count; thisLinearCoord++)
            {
                for (int face = 0; face < 6; face++)
                {
                    var orientation = (MC_Define_Orientation.Enum_Orientation)face;
                    Vector3Int thisRelaCoord = MC_Util_Math.Micro_LinearToRela(chunkMacroData.chunkSize, thisLinearCoord);
                    Vector3Int targetRelaCoord = thisRelaCoord + MC_Define_Orientation.Vec_Orientation[face];

                    if (chunkMicroData.isSolid(chunkMacroData.chunkSize, targetRelaCoord))
                        continue;

                    MC_Define_ChunkRenderData.GetQuad(thisRelaCoord, verticesIndexHead, orientation, out MC_Define_QuadMeshBuffer quadMeshBuffer);

                    vertices.AddRange(quadMeshBuffer.vertices);
                    triangles[0].AddRange(quadMeshBuffer.triangles);
                    uvs.AddRange(quadMeshBuffer.uvs);
                    verticesIndexHead += 4;

                    // Light
                    for (int i = 0; i < 4; i++)
                        colors.Add(MC_Define_ChunkRenderData.GetVoxelLight(chunkMicroData.GetVoxel(thisLinearCoord).Light[face]));

                }
            }

            Vector3 chunkWorldPos = MC_Util_Math.Macro_LogicToWorld(chunkMacroData.chunkSize, chunkMacroData.chunkLogicPos);
            chunkRenderData.Set(vertices.ToArray(), colors.ToArray(), uvs.ToArray(), triangles, chunkWorldPos); // ⬅ 传入颜色
        }


        public override void PostProcess()
        {
            PostProcess_ApplyLightDimming();
        }

        //光线衰减函数
        void PostProcess_ApplyLightDimming()
        {
            //先遍历一遍Voxel数据，寻找亮度
        }

        //寻找太阳照射面
        void PostProcess_ProcessSunlitSurface()
        {
            //如果碰到亮度更高的，会被覆盖
        }
    }
}
