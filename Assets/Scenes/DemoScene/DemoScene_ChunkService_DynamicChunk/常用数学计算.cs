using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class 常用数学计算
{
    public static Vector3 LogicToWorld(Vector3Int logicPos)
    {
        return new Vector3(logicPos.x * 区块全局设置.区块大小.x, logicPos.y * 区块全局设置.区块大小.y, logicPos.z * 区块全局设置.区块大小.z);
    }

    public static Vector3Int WorldToLogic(Vector3 relaPos)
    {
        return new Vector3Int((int)relaPos.x / 区块全局设置.区块大小.x, (int)relaPos.y / 区块全局设置.区块大小.y, (int)relaPos.z / 区块全局设置.区块大小.z);
    }

}
