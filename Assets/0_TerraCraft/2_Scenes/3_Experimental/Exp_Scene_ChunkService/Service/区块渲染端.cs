using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public class 区块渲染端 : TC.Gameplay.TC_Mono_Base
    {
        public 区块数据端 数据端;

        [ReadOnly] public int 渲染的区块数;
        [ReadOnly] public int 实时面数;

        private Plane[] 视锥体平面;

        public override void Update_GameState_Playing()
        {
            if (数据端 == null || 数据端.AllChunks == null)
                return;

            // 计算摄像机视锥体平面
            视锥体平面 = GeometryUtility.CalculateFrustumPlanes(数据端.Player_Camera);

            RenderChunk();
        }


        void RenderChunk()
        {
            渲染的区块数 = 0;
            int temp = 0;

            // 获取排序好的逻辑坐标列表
            var sortedLogicPosList = 数据端.AllChunks.GetChunksSortedByDistance();

            foreach (var logicPos in sortedLogicPosList)
            {
                if (数据端.AllChunks.TryGetChunk(logicPos, out var chunk))
                {
                    if (chunk.渲染数据.chunkMesh == null || chunk.渲染数据.chunkMaterial == null)
                        continue;

                    Vector3 chunkWorldPos = 常用数学计算.LogicToWorld(数据端.区块全局数据, chunk.myLogicPos);
                    Vector3 size = 数据端.区块全局数据.GetChunkSize();
                    Bounds bounds = new Bounds(chunkWorldPos + size * 0.5f, size);

                    // 判断是否在摄像机视锥体内
                    if (GeometryUtility.TestPlanesAABB(视锥体平面, bounds))
                    {
                        Graphics.DrawMesh(chunk.渲染数据.chunkMesh, chunk.渲染数据.matrix, chunk.渲染数据.chunkMaterial, 0, 数据端.Player_Camera);
                        temp++;
                    }
                }
            }

            渲染的区块数 = temp;
            实时面数 = 渲染的区块数 * 12; // 12是一个区块面数示例，根据实际调整
        }


    }
}
