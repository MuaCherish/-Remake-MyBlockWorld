using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 区块加载端 : MonoBehaviour
{
    public 区块数据端 数据端;

    private void Start()
    {
        Service_DynamicChunkLoad();
    }

    private void Update()
    {
        //Service_DynamicChunkLoad();
    }

    private void Service_DynamicChunkLoad()
    {
        // 获取需要生成的区块逻辑坐标列表
        List<Vector3Int> chunksToGenerate = 获取待生成区块逻辑坐标();

        // 提前返回：如果没有要生成的区块，跳过
        if (chunksToGenerate.Count == 0)
            return;

        // 遍历列表，生成区块并添加到 AllChunks 中
        foreach (var logicPos in chunksToGenerate)
        {
            区块 chunk = 区块对象池.Get(logicPos, 数据端);
            数据端.AllChunks.Add(logicPos, chunk);
            Debug.Log($"[区块加载端] 区块 {logicPos} 已添加到 AllChunks");
        }

        // 此处保留清理远距离区块的空间，可在后续添加
        // ...
    }

    /// <summary>
    /// 获取玩家视野范围内待生成的区块逻辑坐标（排除已存在的）
    /// </summary>
    private List<Vector3Int> 获取待生成区块逻辑坐标()
    {
        List<Vector3Int> result = new List<Vector3Int>();

        Vector3 playerWorldPos = 数据端.Player.position;
        Vector3Int playerLogicPos = 常用数学计算.WorldToLogic(playerWorldPos);

        int range = 区块全局设置.渲染半径; // 逻辑坐标半径，生成 ±2 范围内的区块

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                for (int z = -range; z <= range; z++)
                {
                    Vector3Int logicPos = playerLogicPos + new Vector3Int(x, y, z);
                    if (!数据端.AllChunks.ContainsKey(logicPos))
                    {
                        result.Add(logicPos);
                    }
                }
            }
        }

        return result;
    }
}
