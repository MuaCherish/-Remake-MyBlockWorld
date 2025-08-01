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
            
            chunkMacroData.chunkLogicPos = initData.chunkLogicPos;//MacroData
            chunkMacroData.chunkSize = initData.chunkSize;//MacroData
            chunkMicroData.Initialize(chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y * chunkMacroData.chunkSize.z);//MicroData
            chunkRenderData.SetMaterials(initData.chunkMaterials);//RendererData
            chunkRenderer = renderer;//Renderer

            CalculateData(); //计算Voxel数据
            CalculateGrid(); //计算网格数据
            PostProcess();   //后处理-亮度衰减
            PushData();      //提交数据
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
        /// 数据后处理
        /// </summary>
        public virtual void PostProcess() { }

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

    }
}