using UnityEngine;

public class 渲染单个区块 : MonoBehaviour
{
    public 区块数据端 数据端;

    private 区块 chunk;

    private void Start()
    {
        Vector3Int pos = Vector3Int.zero;

        chunk = new 区块(pos, 数据端);
    }

    private void OnRenderObject()
    {
        if (chunk != null)
        {
            Graphics.DrawMesh(chunk.chunkMesh, chunk.matrix, chunk.chunkMaterial, 0);
        }
    }
}
