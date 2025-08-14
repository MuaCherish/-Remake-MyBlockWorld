using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public static class 区块对象池
    {
        private static Stack<可面剔除的无数据区块> pool = new Stack<可面剔除的无数据区块>();

        /// <summary>
        /// 从池中取出一个区块，如果没有则创建一个新的
        /// </summary>
        public static 可面剔除的无数据区块 Get(Vector3Int logicPos, 区块数据端 数据端)
        {
            可面剔除的无数据区块 chunk;

            if (pool.Count > 0)
            {
                chunk = pool.Pop();
                chunk.myLogicPos = logicPos;
                chunk.渲染数据.chunkMaterial = 数据端.Mat_Chunk;
                chunk.Caculate(); // 重新计算位置矩阵、Mesh
            }
            else
            {
                chunk = new 可面剔除的无数据区块(logicPos, 数据端);
            }

            return chunk;
        }

        /// <summary>
        /// 回收区块对象以备复用
        /// </summary>
        public static void Recycle(可面剔除的无数据区块 chunk)
        {
            if (chunk.渲染数据.chunkMesh != null)
            {
                Mesh对象池.Recycle(chunk.渲染数据.chunkMesh); // 不 destroy，回收！
                chunk.渲染数据.chunkMesh = null;
            }

            chunk.渲染数据.matrix = Matrix4x4.identity;
            pool.Push(chunk);
        }


    }

}
