using System.Collections.Generic;
using UnityEngine;

public static class 区块对象池 
{
    private static Stack<区块> pool = new Stack<区块>();

    /// <summary>
    /// 从池中取出一个区块，如果没有则创建一个新的
    /// </summary>
    public static 区块 Get(Vector3Int logicPos, 区块数据端 数据端)
    {
        区块 chunk;

        if (pool.Count > 0)
        {
            chunk = pool.Pop();
            chunk.myLogicPos = logicPos;
            chunk.chunkMaterial = 数据端.Mat_Chunk;
            chunk.Caculate(); // 重新计算位置矩阵、Mesh
        }
        else
        {
            chunk = new 区块(logicPos, 数据端);
        }

        数据端.ComputeReadyChunks.Add(logicPos); // 注册回渲染队列
        return chunk;
    }

    /// <summary>
    /// 回收区块对象以备复用
    /// </summary>
    public static void Recycle(区块 chunk)
    {
        // 如果 chunk.mesh 有大量数据，也可以选择清除 mesh
        chunk.chunkMesh = null;
        chunk.matrix = Matrix4x4.identity;

        // 注意：不要清空 myLogicPos（视逻辑需求）
        pool.Push(chunk);
    }
}
