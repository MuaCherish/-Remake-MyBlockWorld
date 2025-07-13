using Homebrew;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MC_Test
{
    public class Test_ChunkUpdater : MonoBehaviour
    {

        #region 变量

        [Foldout("引用", true)]
        public Camera cam;

        [Foldout("对象池", true)]
        public GameObject PoolRoot;
        public GameObject chunkPrefeb;
        [Header("渲染半径")] public int RenderRange;


        #endregion


        #region 周期函数 

        private void Start()
        {
            MC_Struct_Chunk_Pool.Instance.InitPool(RenderRange, chunkPrefeb, PoolRoot.transform);

            List<Vector3Int> list = GetChunkCoordsInRange(cam, Vector3.zero, RenderRange, new Vector3(16,16,16));

            foreach (var pos in list)
            {
                MC_Struct_Chunk_Pool.Instance.RegisterChunk(pos);
            }
        }

        private void Update()
        {
            ReferUpdate_CheckPlayerDistance();
        }

        #endregion


        #region 区块更新

        void ReferUpdate_CheckPlayerDistance()
        {
            
        }

        void OnPlayerMovedEnough()
        {

        }

        #endregion


        #region 区块管理函数

        public void ReloadAllChunks()
        {

        }

        void UpdateToRigisterChunk()
        {

        }

        void UpdateToUnRigisterChunk()
        {

        }

        void RegisterOneChunk(Vector3Int chunkCoord)
        {
            
        }

        void UnRegisterOneChunk(Vector3Int chunkCoord)
        {
            MC_Struct_Chunk_Pool.Instance.UnregisterChunk(chunkCoord);
        }

        #endregion


        #region 工具

        List<Vector3Int> GetChunkCoordsInRange(Camera cam, Vector3 centre, int renderRange, Vector3 chunkSize)
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

