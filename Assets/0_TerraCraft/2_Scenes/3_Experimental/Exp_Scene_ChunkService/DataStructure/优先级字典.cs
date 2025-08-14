using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    /// <summary>
    /// 优先级字典：用于高效管理大量区块数据，支持快速坐标查找、批量增删和按玩家距离排序。
    /// 
    /// 核心技术点：
    /// - 使用 Dictionary 实现 O(1) 的逻辑坐标查找和去重，提升访问效率。
    /// - 利用对象池（区块对象池）复用区块实例，避免频繁分配和释放内存，优化性能和GC压力。
    /// - 维护排序缓存，避免每帧重复排序；通过外部调用统一触发排序更新，提升调用效率。
    /// - 基于玩家当前位置的逻辑坐标，对区块进行平方距离排序，避免开方计算，提升计算效率。
    /// - 提供批量添加、删除接口，支持区块范围的自动清理，保证内存占用的合理性。
    /// </summary>

    /// </summary>

    public class 优先级字典
    {
        private Transform PlayerTransform;
        private Dictionary<Vector3Int, 可面剔除的无数据区块> chunkMap = new();
        private 区块数据端 数据端;
        public int Count => chunkMap.Count;


        // 排序缓存
        private List<Vector3Int> sortedChunkKeysCache = new();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="playerTransform"></param>
        /// <param name="数据端"></param>
        public 优先级字典(Transform playerTransform, 区块数据端 数据端)
        {
            PlayerTransform = playerTransform;
            this.数据端 = 数据端;
        }

        /// <summary>
        /// 批量添加区块坐标，在内部通过对象池构建区块，并更新排序缓存
        /// </summary>
        public void AddRange(List<Vector3Int> logicPosList)
        {
            if (logicPosList == null || logicPosList.Count == 0)
                return;

            foreach (var pos in logicPosList)
            {
                if (!chunkMap.ContainsKey(pos))
                {
                    var chunk = 区块对象池.Get(pos, 数据端);
                    chunkMap[pos] = chunk;
                }
            }
        }

        /// <summary>
        /// 批量移除区块
        /// </summary>
        public void Remove(List<Vector3Int> logicPosList)
        {
            foreach (var pos in logicPosList)
            {
                if (chunkMap.TryGetValue(pos, out var chunk))
                {
                    区块对象池.Recycle(chunk); // ✅ 回收整个区块对象（内部自动回收 mesh）
                    chunkMap.Remove(pos);
                }
            }
        }

        /// <summary>
        /// 判断是否包含指定逻辑坐标
        /// </summary>
        public bool Contains(Vector3Int logicPos)
        {
            return chunkMap.ContainsKey(logicPos);
        }

        /// <summary>
        /// 直接返回排序缓存，不再做排序
        /// </summary>
        public List<Vector3Int> GetChunksSortedByDistance()
        {
            return sortedChunkKeysCache;
        }

        /// <summary>
        /// 更新排序缓存，按玩家逻辑坐标排序
        /// </summary>
        public void UpdateSortedCache()
        {
            Vector3Int playerLogicPos = 常用数学计算.WorldToLogic(数据端.区块全局数据, PlayerTransform.position);

            sortedChunkKeysCache = chunkMap.Keys
                .OrderBy(pos => (pos - playerLogicPos).sqrMagnitude)
                .ToList();
        }

        /// <summary>
        /// 获取所有区块（未排序）
        /// </summary>
        public IEnumerable<KeyValuePair<Vector3Int, 可面剔除的无数据区块>> GetAllChunks()
        {
            return chunkMap;
        }

        /// <summary>
        /// 清理所有距离玩家超过渲染范围的区块（根据加载规则）
        ///自动卸载
        /// </summary>
        public void RemoveChunksBeyondRange()
        {
            if (chunkMap.Count == 0)
                return;

            Vector3Int playerLogicPos = 常用数学计算.WorldToLogic(数据端.区块全局数据, PlayerTransform.position);
            int renderRange = 数据端.区块全局数据.逻辑渲染半径;

            int yMin = playerLogicPos.y - 2;
            int yMax = playerLogicPos.y + 1;

            List<Vector3Int> chunksToRemove = new();

            foreach (var kvp in chunkMap)
            {
                Vector3Int pos = kvp.Key;

                // 水平方向超出渲染范围
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


        /// <summary>
        /// 获取区块
        /// </summary>
        /// <param name="logicPos"></param>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public bool TryGetChunk(Vector3Int logicPos, out 可面剔除的无数据区块 chunk)
        {
            return chunkMap.TryGetValue(logicPos, out chunk);
        }

    }
}
