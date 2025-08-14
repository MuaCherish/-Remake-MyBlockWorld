#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

///问题：
///allchunk使用的不是MC_Base_Chunk
namespace Tc.Exp.ChunkService
{
    public enum 渲染模式
    {
        线框模式,
        完整渲染,
    }

    [ExecuteInEditMode]
    public class 场景相机渲染测试器 : MonoBehaviour
    {
        public 渲染模式 当前渲染模式 = 渲染模式.线框模式;

        private static 场景相机渲染测试器 实例;

        [InitializeOnLoadMethod]
        static void 注册到SceneGUI()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnEnable()
        {
            实例 = this;
        }

        private void OnDisable()
        {
            if (实例 == this)
                实例 = null;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (!Application.isPlaying) return;
            if (实例 == null) return;

            var 数据端 = GameObject.FindObjectOfType<区块数据端>();
            if (数据端 == null || 数据端.AllChunks == null || 数据端.Player_Camera == null) return;

            Vector3 size = 数据端.区块全局数据.GetChunkSize();
            Plane[] gameCameraPlanes = GeometryUtility.CalculateFrustumPlanes(数据端.Player_Camera);

            foreach (var kvp in 数据端.AllChunks.GetAllChunks())
            {
                var chunk = kvp.Value;
                if (chunk?.渲染数据 == null) continue;
                if (chunk.渲染数据.chunkMesh == null || chunk.渲染数据.chunkMaterial == null) continue;

                Vector3 worldPos = 常用数学计算.LogicToWorld(数据端.区块全局数据, kvp.Key);
                Bounds bounds = new Bounds(worldPos + size * 0.5f, size);

                if (GeometryUtility.TestPlanesAABB(gameCameraPlanes, bounds))
                {
                    if (实例.当前渲染模式 == 渲染模式.完整渲染)
                    {
                        Graphics.DrawMesh(
                            chunk.渲染数据.chunkMesh,
                            chunk.渲染数据.matrix,
                            chunk.渲染数据.chunkMaterial,
                            0,
                            sceneView.camera
                        );
                    }
                    else if (实例.当前渲染模式 == 渲染模式.线框模式)
                    {
                        Handles.color = Color.green;
                        Handles.DrawWireCube(bounds.center, bounds.size);
                    }
                }
            }

            SceneView.RepaintAll();
        }
    }
}
#endif
