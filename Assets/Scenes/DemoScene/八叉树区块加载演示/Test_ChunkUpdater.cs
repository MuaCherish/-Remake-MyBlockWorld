using Homebrew;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

namespace MC_Test
{
    public class Test_ChunkUpdater : MonoBehaviour
    {

        #region 变量

        [Foldout("引用", true)]
        public Camera cam;
        public Transform centreTarget; // 可选绑定 player


        [Foldout("对象池", true)]
        public GameObject PoolRoot;
        public GameObject chunkPrefeb;
        [Header("渲染半径")] public int RenderRange;


        private Vector3 lastCameraPos;
        private Quaternion lastCameraRot;
        private const float positionThreshold = 0.01f;
        private const float rotationThreshold = 0.1f; // 用角度差比较

        private HashSet<Vector3Int> currentVisibleChunks = new HashSet<Vector3Int>();
        private HashSet<Vector3Int> lastVisibleChunks = new HashSet<Vector3Int>();


        #endregion


        #region 周期函数 

        private void Start()
        {
            MC_Struct_Chunk_Pool.Instance.InitPool(RenderRange, chunkPrefeb, PoolRoot.transform);
            lastCameraPos = cam.transform.position;
            lastCameraRot = cam.transform.rotation;

            UpdateToRigisterChunk(); 
        }

        private void Update()
        {
            ReferUpdate_DynamicChunks();
        }

        #endregion


        #region 区块更新

        void ReferUpdate_DynamicChunks()
        {
            if (!isPlayerMoving())
                return;

            UpdateToRigisterChunk(); // 包含了注册 + 卸载逻辑
        }

        bool isPlayerMoving()
        {
            float posDelta = (cam.transform.position - lastCameraPos).sqrMagnitude;
            float rotDelta = Quaternion.Angle(cam.transform.rotation, lastCameraRot);

            if (posDelta > positionThreshold * positionThreshold || rotDelta > rotationThreshold)
            {
                lastCameraPos = cam.transform.position;
                lastCameraRot = cam.transform.rotation;
                return true;
            }
            return false;
        }


        #endregion


        #region 区块管理函数

        public void ReloadAllChunks()
        {

        }

        void UpdateToRigisterChunk()
        {
            currentVisibleChunks.Clear();

            Vector3Int centerCoord = WorldToChunkCoord(centreTarget.position, new Vector3(16, 16, 16));

            foreach (var coord in GetNewChunkCoordsInRange(cam, centreTarget.position, RenderRange, new Vector3(16, 16, 16)))
                currentVisibleChunks.Add(coord);

            // 注册新 Chunk
            foreach (var pos in currentVisibleChunks)
            {
                if (!lastVisibleChunks.Contains(pos))
                {
                    MC_Struct_Chunk_Pool.Instance.RegisterChunk(pos);
                }
            }

            // 注销旧 Chunk：只移除“距离太远”的
            foreach (var oldPos in lastVisibleChunks)
            {
                if (!currentVisibleChunks.Contains(oldPos))
                {
                    int dx = Mathf.Abs(oldPos.x - centerCoord.x);
                    int dy = Mathf.Abs(oldPos.y - centerCoord.y);
                    int dz = Mathf.Abs(oldPos.z - centerCoord.z);

                    if (dx > RenderRange || dy > RenderRange || dz > RenderRange)
                    {
                        MC_Struct_Chunk_Pool.Instance.UnregisterChunk(oldPos);
                    }
                }
            }

            // 交换引用（或者复制）
            var temp = lastVisibleChunks;
            lastVisibleChunks = currentVisibleChunks;
            currentVisibleChunks = temp;
        }

        /// <summary>
        /// 获取可生成的区块的坐标
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="centre"></param>
        /// <param name="renderRange"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        List<Vector3Int> GetNewChunkCoordsInRange(Camera cam, Vector3 centre, int renderRange, Vector3 chunkSize)
        {
            List<Vector3Int> result = new List<Vector3Int>();

            Vector3Int centerCoord = WorldToChunkCoord(centre, chunkSize);

            // 获取摄像机的视锥体平面
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

            for (int x = -renderRange; x <= renderRange; x++)
                for (int y = -renderRange; y <= renderRange; y++)
                    for (int z = -renderRange; z <= renderRange; z++)
                    {
                        Vector3Int offset = new Vector3Int(x, y, z);
                        Vector3Int coord = centerCoord + offset;

                        // ✅ 新增：过滤逻辑坐标 > 0 的
                        if (coord.y > 0)
                            continue;

                        // 计算 chunk 的世界坐标范围（中心点 + 包围盒）
                        Vector3 chunkWorldCenter = new Vector3(
                            coord.x * chunkSize.x + chunkSize.x / 2f,
                            coord.y * chunkSize.y + chunkSize.y / 2f,
                            coord.z * chunkSize.z + chunkSize.z / 2f
                        );

                        Bounds chunkBounds = new Bounds(chunkWorldCenter, chunkSize);

                        // 判断是否在视野内
                        if (!GeometryUtility.TestPlanesAABB(frustumPlanes, chunkBounds))
                            continue;

                        result.Add(coord);
                    }

            return result;
        }

        #endregion


        #region 工具


        /// <summary>
        /// 将世界坐标转换为逻辑 Chunk 坐标
        /// </summary>
        Vector3Int WorldToChunkCoord(Vector3 worldPos, Vector3 chunkSize)
        {
            return new Vector3Int(
                Mathf.FloorToInt(worldPos.x / chunkSize.x),
                Mathf.FloorToInt(worldPos.y / chunkSize.y),
                Mathf.FloorToInt(worldPos.z / chunkSize.z)
            );
        }


        #endregion


    }

}

