#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using DemoScene_ChunkService_DynamicChunk;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneViewChunkRenderer
{
    static SceneViewChunkRenderer()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (SceneManager.GetActiveScene().name != "DemoScene_ChunkService_DynamicChunk")
            return;

        var 数据端 = Object.FindObjectOfType<区块数据端>();
        if (数据端 == null || 数据端.AllChunks == null || 数据端.AllChunks.Count == 0)
            return;

        // 用玩家摄像机做剔除
        Camera playerCam = 数据端.Player_Camera != null ? 数据端.Player_Camera.GetComponentInChildren<Camera>() : null;
        if (playerCam == null)
            return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        Vector3 chunkSize = 区块全局设置.区块大小;  // 你的区块大小配置

        foreach (var kvp in 数据端.AllChunks)
        {
            var chunk = kvp.Value;

            if (chunk.渲染数据.chunkMesh == null || chunk.渲染数据.chunkMaterial == null)
                continue;

            Vector3 center = 常用数学计算.LogicToWorld(kvp.Key) + chunkSize * 0.5f;
            Bounds bounds = new Bounds(center, chunkSize);

            // 用玩家摄像机视锥剔除
            if (GeometryUtility.TestPlanesAABB(planes, bounds))
            {
                // 这里用Scene视图摄像机渲染，保证Scene视图可见
                Graphics.DrawMesh(chunk.渲染数据.chunkMesh, chunk.渲染数据.matrix, chunk.渲染数据.chunkMaterial, 0, sceneView.camera);
            }
        }

        sceneView.Repaint();
    }
}
#endif
