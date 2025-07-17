using UnityEngine;
using DemoScene_ChunkService_DynamicChunk;

[ExecuteAlways]
public class Scene相机渲染测试器 : MonoBehaviour
{
    public 区块数据端 数据端;

    private void OnDrawGizmos()
    {
        if (数据端 == null || 数据端.AllChunks == null || 数据端.AllChunks.Count == 0)
            return;

        Camera playerCam = 数据端.Player_Camera != null ? 数据端.Player_Camera.GetComponentInChildren<Camera>() : null;
        if (playerCam == null)
            return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        Vector3 chunkSize = 数据端.区块全局数据.GetChunkSize();

        // 计算所有区块的包围盒（大长方体）
        Bounds combinedBounds = new Bounds();
        bool first = true;
        foreach (var kvp in 数据端.AllChunks.GetAllChunks())
        {
            Vector3 center = 常用数学计算.LogicToWorld(数据端.区块全局数据, kvp.Key) + chunkSize * 0.5f;
            Bounds bounds = new Bounds(center, chunkSize);

            if (first)
            {
                combinedBounds = bounds;
                first = false;
            }
            else
            {
                combinedBounds.Encapsulate(bounds);
            }
        }

        // 绘制包围所有区块的大长方体，白色线框
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(combinedBounds.center, combinedBounds.size);

        // 绘制视锥体内的区块绿色线框
        Gizmos.color = Color.green;
        foreach (var kvp in 数据端.AllChunks.GetAllChunks())
        {
            var chunk = kvp.Value;
            if (chunk.渲染数据.chunkMesh == null || chunk.渲染数据.chunkMaterial == null)
                continue;

            Vector3 center = 常用数学计算.LogicToWorld(数据端.区块全局数据, kvp.Key) + chunkSize * 0.5f;
            Bounds bounds = new Bounds(center, chunkSize);

            if (GeometryUtility.TestPlanesAABB(planes, bounds))
            {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.SceneView.RepaintAll();
        }
    }
#endif
}
