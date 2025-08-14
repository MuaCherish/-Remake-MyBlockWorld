using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; // 引入 Stopwatch 计时器

namespace Tc.Exp.ChunkMeshGenerate
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

            // 异常判断
            if (initData.chunkSize.x == 0 || initData.chunkSize.y == 0 || initData.chunkSize.z == 0)
            {
                UnityEngine.Debug.LogWarning($"区块未正确初始化。ChunkSize: {initData.chunkSize}");
                return;
            }
            if (initData.chunkMaterials.Length == 0)
            {
                UnityEngine.Debug.LogWarning($"区块未正确初始化。chunkMaterials.Length: {initData.chunkMaterials.Length}");
                return;
            }

            chunkMacroData.chunkLogicPos = initData.chunkLogicPos;
            chunkMacroData.chunkSize = initData.chunkSize;
            chunkMicroData.Initialize(chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y * chunkMacroData.chunkSize.z);
            chunkRenderData.SetMaterials(initData.chunkMaterials);
            chunkRenderer = renderer;

            CalculateVoxelData(); // 计算Voxel数据
            PostProcess();   // 亮度衰减等后处理
            CalculateGridData(); // 计算网格数据
            PushData();      // 推送渲染数据

        }

        /// <summary>
        /// 虚方法：计算区块微观数据
        /// </summary>
        public virtual void CalculateVoxelData() { }

        /// <summary>
        /// 虚方法：如何计算网格
        /// </summary>
        public virtual void CalculateGridData() { }

        /// <summary>
        /// 数据后处理
        /// </summary>
        public virtual void PostProcess() { }

        /// <summary>
        /// 上传数据用于渲染
        /// </summary>
        public void PushData()
        {
            if (chunkRenderer == null)
            {
                UnityEngine.Debug.LogError("Service_chunkRenderer 未赋值，无法调用 PushData()");
                return;
            }

            if (!chunkRenderData.IsValid)
            {
                UnityEngine.Debug.LogError("渲染数据未准备完毕，无法调用 PushData()");
                UnityEngine.Debug.LogError($"[渲染参数]  Mesh：{chunkRenderData.ChunkMesh}， 材质：{chunkRenderData.ChunkMaterials}， 材质Count：{chunkRenderData.ChunkMaterials.Length}");
                return;
            }

            chunkRenderer.PullData(this);
        }
    }
}
