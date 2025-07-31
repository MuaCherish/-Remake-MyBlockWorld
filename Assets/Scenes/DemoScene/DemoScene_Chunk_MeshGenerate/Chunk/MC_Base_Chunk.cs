using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    /// <summary>
    /// 区块父类
    /// </summary>
    public abstract class MC_Base_Chunk
    {

        public ChunkMacroData chunkMacroData = new ChunkMacroData();    //区块宏观数据
        public ChunkMicroData chunkMicroData = new ChunkMicroData();    //区块微观数据
        public ChunkRenderData chunkRenderData = new ChunkRenderData(); //区块渲染数据
        private IChunkRenderer chunkRenderer;                           //用于回调的自定义渲染器

        /// <summary>
        /// 传递初始化数据+可用的渲染器用于回调
        /// </summary>
        public MC_Base_Chunk(ChunkInitData initData, IChunkRenderer renderer)
        {
            //MacroData
            chunkMacroData.chunkLogicPos = initData.chunkLogicPos;
            chunkMacroData.chunkSize = initData.chunkSize;

            //MicroData
            chunkMicroData.Initialize(chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y * chunkMacroData.chunkSize.z);

            //RendererData
            chunkRenderData.SetMaterials(initData.chunkMaterials);

            //Renderer
            chunkRenderer = renderer;

            CalculateData();
            CalculateGrid();
            PushData();
        }

        /// <summary>
        /// 虚方法：计算区块微观数据
        /// </summary>
        public virtual void CalculateData() { }

        /// <summary>
        /// 虚方法：如何计算网格
        /// </summary>
        public virtual void CalculateGrid() { }

        /// <summary>
        /// 公开方法：上传数据
        /// 用于Chunk计算完毕回调给渲染器
        /// </summary>
        public void PushData()
        {
            if (chunkRenderer == null)
            {
                Debug.LogError("Service_chunkRenderer 未赋值，无法调用 PushData()");
                return;
            }

            if (!chunkRenderData.IsValid)
            {
                Debug.LogError("渲染数据未准备完毕，无法调用 PushData()");
                Debug.LogError($"[渲染参数]  Mesh：{chunkRenderData.ChunkMesh}， 材质：{chunkRenderData.ChunkMaterials}， 材质Count：{chunkRenderData.ChunkMaterials.Length}");
                return;
            }

            // 传入当前区块实例
            chunkRenderer.PullData(this);
        }

        /// <summary>
        ///  工具方法：三维变一维
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public int LogicToLinear(Vector3Int coord)
        {
            if (coord.x < 0 || coord.x >= chunkMacroData.chunkSize.x ||
                coord.y < 0 || coord.y >= chunkMacroData.chunkSize.y ||
                coord.z < 0 || coord.z >= chunkMacroData.chunkSize.z)
            {
                Debug.LogError($"索引越界: ({coord.x},{coord.y},{coord.z}) 超出范围 {chunkMacroData.chunkSize}");
                return -1;
            }
            return coord.x + chunkMacroData.chunkSize.x * (coord.y + chunkMacroData.chunkSize.y * coord.z);
        }

        /// <summary>
        /// 工具方法：一维变三维
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3Int LinearToLogic(int index)
        {
            if (index < 0 || index >= chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y * chunkMacroData.chunkSize.z)
            {
                Debug.LogError($"索引越界: index={index} 超出范围 {chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y * chunkMacroData.chunkSize.z}");
                return Vector3Int.zero;
            }
            int x = index % chunkMacroData.chunkSize.x;
            int y = (index / chunkMacroData.chunkSize.x) % chunkMacroData.chunkSize.y;
            int z = index / (chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y);
            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// 工具方法：判断是否是固体
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool isSolid(Vector3Int coord)
        {
            if (chunkMicroData.GetVoxel(LogicToLinear(coord)).Type == 1)
                return true;
            else
                return false;
        }
    }
}