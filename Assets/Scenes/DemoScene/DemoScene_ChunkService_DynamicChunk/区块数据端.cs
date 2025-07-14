using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 区块数据端 : MonoBehaviour
{
    //存储所有区块的变量AllChunks
    //涉及操作有大量查询，单纯添加和单纯删除
    public Dictionary<Vector3Int, 区块> AllChunks;
    public Material Mat_Chunk;

    //计算完成的区块数据结构
    //涉及操作有大量遍历，单纯添加和中间删除，根据绝对坐标与玩家距离由近到远自动排序
    public Transform Player;
    public 距离排序表 ComputeReadyChunks;

    //在视野中的区块数据结构
    //涉及操作有先进先出
    //public Queue<Vector3Int> VisibleChunksQueue;

    private void Awake()
    {
        ComputeReadyChunks = new 距离排序表(Player);
        AllChunks = new Dictionary<Vector3Int, 区块>();
    }

    void OnDrawGizmos()
    {
        if (AllChunks == null || Camera.main == null)
            return;

        Gizmos.color = Color.green;

        // 获取摄像机视锥体平面
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        foreach (var kvp in AllChunks)
        {
            Vector3Int logicPos = kvp.Key;
            Vector3 size = 区块全局设置.区块大小;
            Vector3 worldPos = 常用数学计算.LogicToWorld(logicPos);
            Bounds bounds = new Bounds(worldPos + size * 0.5f, size);

            // 判断是否在摄像机视锥体内
            if (GeometryUtility.TestPlanesAABB(frustumPlanes, bounds))
            {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }


}






