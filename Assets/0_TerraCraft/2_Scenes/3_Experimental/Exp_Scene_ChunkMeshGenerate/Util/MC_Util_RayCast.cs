using UnityEngine;

namespace Tc.Exp.ChunkService
{
    /// <summary>
    /// 射线检测类
    /// </summary>
    public class MC_Util_RayCast
    {
        /// <summary>
        /// 射线检测函数
        /// </summary>
        /// <param name="_raCastInitInfo"></param>
        /// <returns></returns>
        public static MC_Define_RayCast_Result RayCast(区块数据端 数据端,MC_Define_RayCast_InitInfo _raCastInitInfo)
        {
            return new MC_Define_RayCast_Result();
        }
    }

    /// <summary>
    /// 射线检测初始化数据
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
    /// 射线检测返回结构体
    /// </summary>
    [System.Serializable]
    public struct MC_Define_RayCast_Result
    {
        /// <summary>
        /// 是否命中: 0没有命中, 1命中方块, 2命中实体
        /// </summary>
        public byte isHit;
        public Vector3 rayOrigin; // 射线起点
        public Vector3 hitPoint;// 打中点坐标
        public Vector3 hitPoint_Previous;// 打中前一点坐标
        public byte blockType;  // 打中方块类型
        public Vector3 hitNormal;// 打中法线方向
        public float rayDistance; // 射线距离
        //public EntityInfo targetEntityInfo;// 目标实体（可为空）

        // 构造函数
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
         
        // 覆盖ToString方法，用于打印输出
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

    //搜索模式
    [System.Serializable]
    public enum MC_RayCast_FindType
    {
        AllFind,
        OnlyFindBlock,
        OnlyFindEntity,
    }

}