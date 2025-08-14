using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    /// <summary>
    /// �����е�����΢�����ݼ���
    /// </summary>
    [Serializable]
    public class ChunkMicroData
    {
        [SerializeField]
        private MC_Define_Class_VoxelState[] voxelMap;

        /// <summary>
        /// ��ǰ�������������
        /// </summary>
        public int Count => voxelMap?.Length ?? 0;

        /// <summary>
        /// ��ʼ���������ݣ�����Ϊָ����С��
        /// </summary>
        /// <param name="size">������������һ��Ϊ �������ߡ��</param>
        public void Initialize(int size)
        {
            voxelMap = new MC_Define_Class_VoxelState[size];
            for (int i = 0; i < size; i++)
            {
                voxelMap[i] = MC_Define_Class_VoxelState.Default; // ʹ�ýṹ���Ĭ��ֵ����
            }
        }

        /// <summary>
        /// ����ָ�����ص�ֵ���ɲ������ã�
        /// </summary>
        public void SetVoxel(
            int index,
            byte? type = null,
            MC_Define_Config_Orientation.Enum_Orientation? orientation = null,
            byte[] lightArray = null,
            int? lightDir = null,
            byte? lightValue = null)
        {
            if (!IsValidIndex(index))
            {
                //Debug.LogWarning($"SetVoxel index out of range: {index}");
                return;
            }

            ref MC_Define_Class_VoxelState v = ref voxelMap[index];

            if (type.HasValue)
                v.Type = type.Value;

            if (orientation.HasValue)
                v.Orientation = orientation.Value;

            if (lightArray != null && lightArray.Length == 6)
            {
                Array.Copy(lightArray, v.Light, 6); // ��������
            }
            else if (lightDir.HasValue && lightValue.HasValue)
            {
                if (lightDir.Value >= 0 && lightDir.Value < 6)
                    v.Light[lightDir.Value] = lightValue.Value; // ��������
                else
                    Debug.LogWarning($"Invalid light direction: {lightDir.Value}");
            }
        }


        /// <summary>
        /// ��ȡָ������
        /// </summary>
        public MC_Define_Class_VoxelState GetVoxel(int index)
        {
            if (IsValidIndex(index))
                return voxelMap[index];

            //Debug.LogWarning($"GetVoxel index out of range: {index}");
            return MC_Define_Class_VoxelState.Default;
        }

        /// <summary>
        /// ��������Ƿ���Ч
        /// </summary>
        private bool IsValidIndex(int index) => voxelMap != null && index >= 0 && index < voxelMap.Length;

        /// <summary>
        /// �ж�ָ���߼�λ���Ƿ�Խ��
        /// </summary>
        /// <param name="chunkSize">����ߴ磨���ߡ��</param>
        /// <param name="logicPos">�����߼�����</param>
        /// <returns>�Ƿ�Խ��</returns>
        public bool IsOutOfIndex(Vector3Int chunkSize, Vector3Int logicPos)
        {
            return logicPos.x < 0 || logicPos.x >= chunkSize.x ||
                   logicPos.y < 0 || logicPos.y >= chunkSize.y ||
                   logicPos.z < 0 || logicPos.z >= chunkSize.z;
        }

        /// <summary>
        /// ���߷������ж��Ƿ��ǹ���
        /// </summary>
        public bool isSolid(Vector3Int chunkSize, Vector3Int thisRelaPos)
        {
            //��ǰ����-����Խ��һ�ɻ��ƣ�������Գ��Լ��Ŀ���������Ӧλ�ã�
            if (IsOutOfIndex(chunkSize, thisRelaPos))
                return false;

            //�ǿ��������
            if (GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, thisRelaPos)).Type == MC_Define_Config_VoxelId.Air ||
                GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, thisRelaPos)).Type == MC_Define_Config_VoxelId.Water
                )
                return false;
            else
                return true;
        }


        /// <summary>
        /// ���߷������ж��Ƿ���Ҫ����
        /// True:����
        /// False:������
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool isNeedDrawQuad(Vector3Int chunkSize, Vector3Int thisRelaCoord, Vector3Int targetRelaCoord)
        {

            byte thisType = GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, thisRelaCoord)).Type;
            byte targetType = GetVoxel(MC_Util_Math.Micro_RelaToLinear(chunkSize, targetRelaCoord)).Type;

            //��ǰ����-�±����
            if (thisType != MC_Define_Config_VoxelId.Air && IsOutOfIndex(chunkSize, targetRelaCoord))
                return true;

            //��ǰ����-�����͸������
            if (isSolid(chunkSize, thisRelaCoord) && !isSolid(chunkSize, targetRelaCoord))
                return true;

            return false;
        }

    }


}
