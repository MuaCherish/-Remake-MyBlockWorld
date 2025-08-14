using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; // ���� Stopwatch ��ʱ��

namespace Tc.Exp.ChunkMeshGenerate
{
    /// <summary>
    /// ���鸸��
    /// </summary>
    public abstract class MC_Base_Chunk
    {
        public ChunkMacroData chunkMacroData = new ChunkMacroData();    //����������
        public ChunkMicroData chunkMicroData = new ChunkMicroData();    //����΢������
        public ChunkRenderData chunkRenderData = new ChunkRenderData(); //������Ⱦ����
        private IChunkRenderer chunkRenderer;                           //���ڻص����Զ�����Ⱦ��

        /// <summary>
        /// ���ݳ�ʼ������+���õ���Ⱦ�����ڻص�
        /// </summary>
        public MC_Base_Chunk(ChunkInitData initData, IChunkRenderer renderer)
        {

            // �쳣�ж�
            if (initData.chunkSize.x == 0 || initData.chunkSize.y == 0 || initData.chunkSize.z == 0)
            {
                UnityEngine.Debug.LogWarning($"����δ��ȷ��ʼ����ChunkSize: {initData.chunkSize}");
                return;
            }
            if (initData.chunkMaterials.Length == 0)
            {
                UnityEngine.Debug.LogWarning($"����δ��ȷ��ʼ����chunkMaterials.Length: {initData.chunkMaterials.Length}");
                return;
            }

            chunkMacroData.chunkLogicPos = initData.chunkLogicPos;
            chunkMacroData.chunkSize = initData.chunkSize;
            chunkMicroData.Initialize(chunkMacroData.chunkSize.x * chunkMacroData.chunkSize.y * chunkMacroData.chunkSize.z);
            chunkRenderData.SetMaterials(initData.chunkMaterials);
            chunkRenderer = renderer;

            CalculateVoxelData(); // ����Voxel����
            PostProcess();   // ����˥���Ⱥ���
            CalculateGridData(); // ������������
            PushData();      // ������Ⱦ����

        }

        /// <summary>
        /// �鷽������������΢������
        /// </summary>
        public virtual void CalculateVoxelData() { }

        /// <summary>
        /// �鷽������μ�������
        /// </summary>
        public virtual void CalculateGridData() { }

        /// <summary>
        /// ���ݺ���
        /// </summary>
        public virtual void PostProcess() { }

        /// <summary>
        /// �ϴ�����������Ⱦ
        /// </summary>
        public void PushData()
        {
            if (chunkRenderer == null)
            {
                UnityEngine.Debug.LogError("Service_chunkRenderer δ��ֵ���޷����� PushData()");
                return;
            }

            if (!chunkRenderData.IsValid)
            {
                UnityEngine.Debug.LogError("��Ⱦ����δ׼����ϣ��޷����� PushData()");
                UnityEngine.Debug.LogError($"[��Ⱦ����]  Mesh��{chunkRenderData.ChunkMesh}�� ���ʣ�{chunkRenderData.ChunkMaterials}�� ����Count��{chunkRenderData.ChunkMaterials.Length}");
                return;
            }

            chunkRenderer.PullData(this);
        }
    }
}
