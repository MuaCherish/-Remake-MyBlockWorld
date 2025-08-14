using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    /// <summary>
    /// 优先级字典：用于高效管理大量区块数据，支持快速坐标查找、批量增删和按玩家距离排序。
    /// 
    /// 注意事项：
    /// 1. 每次一轮结束需要手动掉哟UpdateSortedCache更新排序
    /// </summary>
    public class MC_Define_Class_PriorityDictionary
    {
        private Vector3Int ChunkSize;
        private Transform PlayerTransform;
        private Dictionary<Vector3Int, MC_Base_Chunk> chunkMap = new();
        public int Count => chunkMap.Count;

        // 排序缓存
        private List<Vector3Int> sortedChunkKeysCache = new();

        /// <summary>
        /// 初始化
        /// </summary>
        public MC_Define_Class_PriorityDictionary(Transform playerTransform, Vector3Int chunkSize)
        {
            PlayerTransform = playerTransform;
            ChunkSize = chunkSize;
        }

        #region Add 添加

        /// <summary>
        /// 添加单个区块
        /// </summary>
        public void AddOnce(Vector3Int logicPos, MC_Base_Chunk chunk)
        {
            if (!chunkMap.ContainsKey(logicPos))
            {
                chunkMap[logicPos] = chunk;
            }
        }

        /// <summary>
        /// 批量添加区块坐标，并更新排序缓存
        /// </summary>
        public void AddRange(List<MC_DataStruct_PriorityDictionary_ChunkInsertInfo> chunkList)
        {
            if (chunkList == null || chunkList.Count == 0)
                return;

            foreach (var item in chunkList)
            {
                if (!chunkMap.ContainsKey(item.logicPos))
                {
                    chunkMap[item.logicPos] = item.chunk;
                }
            }
        }

        #endregion

        #region Remove 删除

        /// <summary>
        /// 删除单个区块
        /// </summary>
        public void RemoveOnce(Vector3Int logicPos)
        {
            chunkMap.Remove(logicPos);
        }

        /// <summary>
        /// 批量移除区块
        /// </summary>
        public void Remove(List<Vector3Int> logicPosList)
        {
            foreach (var pos in logicPosList)
            {
                chunkMap.Remove(pos);
            }
        }

        /// <summary>
        /// 清理所有距离玩家超过渲染范围的区块（自动卸载）
        /// </summary>
        public void RemoveChunksBeyondRange(int renderRange)
        {
            if (chunkMap.Count == 0)
                return;

            Vector3Int playerLogicPos = MC_Util_Math.Macro_WorldToLogic(ChunkSize, PlayerTransform.position);
            int yMin = playerLogicPos.y - 2;
            int yMax = playerLogicPos.y + 1;

            List<Vector3Int> chunksToRemove = new();

            foreach (var kvp in chunkMap)
            {
                Vector3Int pos = kvp.Key;

                // 水平方向超出渲染范围或垂直方向不在范围内
                if (Mathf.Abs(pos.x - playerLogicPos.x) > renderRange ||
                    Mathf.Abs(pos.z - playerLogicPos.z) > renderRange ||
                    pos.y < yMin || pos.y > yMax)
                {
                    chunksToRemove.Add(pos);
                }
            }

            if (chunksToRemove.Count > 0)
            {
                Remove(chunksToRemove);
            }
        }

        #endregion

        #region Find 查找

        /// <summary>
        /// 判断是否包含指定逻辑坐标
        /// </summary>
        public bool Contains(Vector3Int logicPos)
        {
            return chunkMap.ContainsKey(logicPos);
        }

        /// <summary>
        /// 获取区块
        /// </summary>
        public bool TryGetChunk(Vector3Int logicPos, out MC_Base_Chunk chunk)
        {
            return chunkMap.TryGetValue(logicPos, out chunk);
        }

        /// <summary>
        /// 获取所有区块（未排序）
        /// </summary>
        public IEnumerable<KeyValuePair<Vector3Int, MC_Base_Chunk>> GetAllChunks()
        {
            return chunkMap;
        }

        #endregion

        #region Edit 排序与编辑

        /// <summary>
        /// 更新排序缓存，按玩家逻辑坐标排序
        /// </summary>
        public void UpdateSortedCache()
        {
            Vector3Int playerLogicPos = MC_Util_Math.Macro_WorldToLogic(ChunkSize, PlayerTransform.position);

            sortedChunkKeysCache = chunkMap.Keys
                .OrderBy(pos => (pos - playerLogicPos).sqrMagnitude)
                .ToList();
        }

        #endregion

        #region Other 其他

        /// <summary>
        /// 直接返回排序缓存，不再做排序
        /// </summary>
        public List<Vector3Int> GetChunksSortedByDistance()
        {
            return sortedChunkKeysCache;
        }

        #endregion
    }

    /// <summary>
    /// 封装区块逻辑位置与数据
    /// </summary>
    public class MC_DataStruct_PriorityDictionary_ChunkInsertInfo
    {
        public Vector3Int logicPos;
        public MC_Base_Chunk chunk;
    }
}
