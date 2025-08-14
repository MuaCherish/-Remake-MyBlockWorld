using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    public class Chunk_测试区块 : MC_Base_Chunk
    {
        public Chunk_测试区块(ChunkInitData initData, IChunkRenderer renderer) : base(initData, renderer) { }

        public override void CalculateVoxelData()
        {

            for (int i = 0; i < chunkMicroData.Count; i++)
            {
                chunkMicroData.SetVoxel(i, type: MC_Define_Config_VoxelId.Stone);
            }

        }

        public override void CalculateGridData()
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
                    var orientation = (MC_Define_Config_Orientation.Enum_Orientation)face;
                    Vector3Int thisRelaCoord = MC_Util_Math.Micro_LinearToRela(chunkMacroData.chunkSize, thisLinearCoord);
                    Vector3Int targetRelaCoord = thisRelaCoord + MC_Define_Config_Orientation.Vec_Orientation[face];

                    
                    if (!chunkMicroData.isNeedDrawQuad(chunkMacroData.chunkSize, thisRelaCoord, targetRelaCoord))
                        continue;

                    MC_Define_Config_ChunkRenderData.GetQuad(thisRelaCoord, verticesIndexHead, orientation, out MC_Define_Class_QuadMeshBuffer quadMeshBuffer);

                    vertices.AddRange(quadMeshBuffer.vertices);
                    triangles[0].AddRange(quadMeshBuffer.triangles);
                    uvs.AddRange(quadMeshBuffer.uvs);
                    verticesIndexHead += 4;

                    // Light
                    //byte lightness = (byte)Random.Range(0, 15);
                    byte lightness = chunkMicroData.GetVoxel(thisLinearCoord).Light[face];
                    for (int i = 0; i < 4; i++)
                        colors.Add(MC_Define_Config_ChunkRenderData.GetVoxelLight(lightness));

                }
            }

            Vector3 chunkWorldPos = MC_Util_Math.Macro_LogicToWorld(chunkMacroData.chunkSize, chunkMacroData.chunkLogicPos);
            chunkRenderData.Set(vertices.ToArray(), colors.ToArray(), uvs.ToArray(), triangles, chunkWorldPos); // ⬅ 传入颜色
        }

        public override void PostProcess()
        {
            PostProcess_CaculateLight();
        }

        //光线衰减函数
        void PostProcess_CaculateLight()
        {

            int maxLight = 14;
            Vector3Int center = new Vector3Int(15, 0, 15);
            Vector3Int chunkSize = chunkMacroData.chunkSize; // 应该是 32x1x32

            for (int x = 0; x < chunkSize.x; x++)
            {
                for (int z = 0; z < chunkSize.z; z++)
                {
                    int manhattanDistance = Mathf.Abs(x - center.x) + Mathf.Abs(z - center.z);
                    int light = maxLight - manhattanDistance;

                    if (light > 0)
                    {
                        int index = MC_Util_Math.Micro_RelaToLinear(chunkSize, new Vector3Int(x, 0, z));
                        chunkMicroData.SetVoxel(
                            index,
                            lightDir: (int)MC_Define_Config_Orientation.Enum_Orientation.Top,
                            lightValue: (byte)light
                        );
                    }
                }
            }




            //先遍历一遍Voxel数据，寻找所有可自发光方块加入队列

            //遍历队列使用BFS重设每个方块的顶点亮度

            //尝试获取区块最顶上的方块(从上往下遍历)并使用BFS计算衰减
        }



    }
}
