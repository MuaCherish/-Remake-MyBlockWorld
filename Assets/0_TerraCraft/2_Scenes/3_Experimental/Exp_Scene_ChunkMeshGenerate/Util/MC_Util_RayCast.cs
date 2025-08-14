using UnityEngine;

namespace Tc.Exp.ChunkService
{
    /// <summary>
    /// ���߼����
    /// </summary>
    public class MC_Util_RayCast
    {
        /// <summary>
        /// ���߼�⺯��
        /// </summary>
        /// <param name="_raCastInitInfo"></param>
        /// <returns></returns>
        public static MC_Define_RayCast_Result RayCast(�������ݶ� ���ݶ�,MC_Define_RayCast_InitInfo _raCastInitInfo)
        {
            return new MC_Define_RayCast_Result();
        }
    }

    /// <summary>
    /// ���߼���ʼ������
    /// </summary>
    public class MC_Define_RayCast_InitInfo
    {
        //public MC_RayCast_FindType _FindType;
        public Vector3 _origin;
        public Vector3 _direct;
        public float _maxDistance;
        public int castingEntityId;
        public float checkIncrement;

        public MC_Define_RayCast_InitInfo() { }

        public MC_Define_RayCast_InitInfo(
            //MC_RayCast_FindType findType,
            Vector3 origin,
            Vector3 direct,
            float maxDistance,
            int castingEntityId,
            float checkIncrement)
        {
            //this._FindType = findType;
            this._origin = origin;
            this._direct = direct;
            this._maxDistance = maxDistance;
            this.castingEntityId = castingEntityId;
            this.checkIncrement = checkIncrement;
        }
    }


    /// <summary>
    /// ���߼�ⷵ�ؽṹ��
    /// </summary>
    [System.Serializable]
    public struct MC_Define_RayCast_Result
    {
        /// <summary>
        /// �Ƿ�����: 0û������, 1���з���, 2����ʵ��
        /// </summary>
        public byte isHit;
        public Vector3 rayOrigin; // �������
        public Vector3 hitPoint;// ���е�����
        public Vector3 hitPoint_Previous;// ����ǰһ������
        public byte blockType;  // ���з�������
        public Vector3 hitNormal;// ���з��߷���
        public float rayDistance; // ���߾���
        //public EntityInfo targetEntityInfo;// Ŀ��ʵ�壨��Ϊ�գ�

        // ���캯��
        public MC_Define_RayCast_Result(byte _isHit, Vector3 _rayOrigin, Vector3 _hitPoint, Vector3 _hitPoint_Previous, byte _blockType, Vector3 _hitNormal, float _rayDistance)
        { 
            this.isHit = _isHit;
            this.rayOrigin = _rayOrigin;
            this.hitPoint = _hitPoint;
            this.hitPoint_Previous = _hitPoint_Previous;
            this.blockType = _blockType;
            this.hitNormal = _hitNormal;
            this.rayDistance = _rayDistance;
            //this.targetEntityInfo = targetEntityInfo;
        }
         
        // ����ToString���������ڴ�ӡ���
        public override string ToString()
        {
            return $"RayCastStruct: \n" +
                   $"  Is Hit: {isHit}\n" +
                   $"  Ray Origin: {rayOrigin}\n" +
                   $"  Hit Point: {hitPoint}\n" +
                   $"  Previous Hit Point: {hitPoint_Previous}\n" +
                   $"  Block Type: {blockType}\n" +
                   $"  Hit Normal: {hitNormal}\n" +
                   $"  Ray Distance: {rayDistance}\n";
                   //$"  Target Entity: {targetEntityInfo._id}, {targetEntityInfo._name}, {targetEntityInfo._obj}";
        }
    }

    //����ģʽ
    [System.Serializable]
    public enum MC_RayCast_FindType
    {
        AllFind,
        OnlyFindBlock,
        OnlyFindEntity,
    }

}