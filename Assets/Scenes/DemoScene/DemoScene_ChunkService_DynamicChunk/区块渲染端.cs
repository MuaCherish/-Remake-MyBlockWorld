using UnityEngine;

public class 区块渲染端 : MonoBehaviour
{
    public 区块数据端 数据端;
    public Camera 渲染摄像机;

    private Plane[] 视锥体平面;

    private void Start()
    {
        if (渲染摄像机 == null)
        {
            渲染摄像机 = Camera.main;
        }
    }

    private void Update()
    {
        if (数据端 == null || 数据端.AllChunks == null)
            return;

        // 计算摄像机视锥体平面
        视锥体平面 = GeometryUtility.CalculateFrustumPlanes(渲染摄像机);

        RenderChunk();
    }

    void RenderChunk()
    {
        foreach (var kvp in 数据端.AllChunks)
        {
            区块 chunk = kvp.Value;
            if (chunk.chunkMesh == null || chunk.chunkMaterial == null)
                continue;

            // 构造区块AABB包围盒（区块世界坐标 + 区块大小）
            Vector3 chunkWorldPos = 常用数学计算.LogicToWorld(chunk.myLogicPos);
            Vector3 size = 区块全局设置.区块大小;
            Bounds bounds = new Bounds(chunkWorldPos + size * 0.5f, size);

            // 判断是否在视锥体内
            if (GeometryUtility.TestPlanesAABB(视锥体平面, bounds))
            {
                // 调用DrawMesh绘制
                Graphics.DrawMesh(chunk.chunkMesh, chunk.matrix, chunk.chunkMaterial, 0, 渲染摄像机);
            }
        }
    }
}
