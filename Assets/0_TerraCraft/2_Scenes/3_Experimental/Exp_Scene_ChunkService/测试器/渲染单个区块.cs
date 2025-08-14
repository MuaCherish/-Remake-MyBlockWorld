using UnityEngine;

namespace Tc.Exp.ChunkService
{

    public class 渲染单个区块 : MonoBehaviour
    {
        public 区块数据端 数据端;

        private 可面剔除的无数据区块 chunk;

        private void Start()
        {
            Vector3Int pos = Vector3Int.zero;

            chunk = new 可面剔除的无数据区块(pos, 数据端);
        }

        private void OnRenderObject()
        {
            if (chunk != null)
            {
                Graphics.DrawMesh(chunk.渲染数据.chunkMesh, chunk.渲染数据.matrix, chunk.渲染数据.chunkMaterial, 0);
            }
        }
    }

}
