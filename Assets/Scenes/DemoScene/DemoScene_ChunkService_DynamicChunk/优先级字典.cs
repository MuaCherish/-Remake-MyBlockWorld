using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    /// <summary>
    /// 优先级字典：支持 O(1) 坐标查询 + 按优先级排序迭代。
    /// </summary>
    public class 优先级字典
    {
        /// <summary>
        /// 内部条目：记录区块、逻辑坐标、优先级。
        /// </summary>
        public class Entry
        {
            public Vector3Int logicPos;
            public 区块 chunk;
            public float priority;

            public Entry(Vector3Int pos, 区块 chunk, float priority)
            {
                this.logicPos = pos;
                this.chunk = chunk;
                this.priority = priority;
            }
        }

        // 逻辑坐标 → 区块条目的快速查找字典
        private readonly Dictionary<Vector3Int, Entry> dict = new();

        // 按优先级排序的条目集合（SortedSet）
        private readonly SortedSet<Entry> sortedSet;

        /// <summary>
        /// 比较器：用于根据优先级对条目排序。
        /// 若优先级相等，则使用逻辑坐标的哈希值打散。
        /// </summary>
        private class EntryComparer : IComparer<Entry>
        {
            public int Compare(Entry a, Entry b)
            {
                int cmp = a.priority.CompareTo(b.priority);
                if (cmp == 0)
                    cmp = a.logicPos.GetHashCode().CompareTo(b.logicPos.GetHashCode());
                return cmp;
            }
        }

        /// <summary>
        /// 构造函数：初始化排序集合。
        /// </summary>
        public 优先级字典()
        {
            sortedSet = new SortedSet<Entry>(new EntryComparer());
        }

        /// <summary>
        /// 添加或更新一个区块条目。
        /// 若已存在该坐标，则更新其区块和优先级；否则新建。
        /// </summary>
        /// <param name="pos">逻辑坐标</param>
        /// <param name="chunk">区块实例</param>
        /// <param name="priority">优先级值</param>
        public void AddOrUpdate(Vector3Int pos, 区块 chunk, float priority)
        {
            if (dict.TryGetValue(pos, out var existing))
            {
                sortedSet.Remove(existing);
                existing.chunk = chunk;
                existing.priority = priority;
                sortedSet.Add(existing);
            }
            else
            {
                var entry = new Entry(pos, chunk, priority);
                dict[pos] = entry;
                sortedSet.Add(entry);
            }
        }

        /// <summary>
        /// 移除指定逻辑坐标对应的条目。
        /// </summary>
        /// <param name="pos">逻辑坐标</param>
        /// <returns>是否成功移除</returns>
        public bool Remove(Vector3Int pos)
        {
            if (dict.TryGetValue(pos, out var entry))
            {
                sortedSet.Remove(entry);
                dict.Remove(pos);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据逻辑坐标获取区块。
        /// </summary>
        /// <param name="pos">逻辑坐标</param>
        /// <returns>对应的区块，或 null</returns>
        public 区块 GetChunk(Vector3Int pos)
        {
            return dict.TryGetValue(pos, out var entry) ? entry.chunk : null;
        }

        /// <summary>
        /// 判断是否包含指定坐标的条目。
        /// </summary>
        /// <param name="pos">逻辑坐标</param>
        /// <returns>是否存在</returns>
        public bool Contains(Vector3Int pos)
        {
            return dict.ContainsKey(pos);
        }

        /// <summary>
        /// 清空所有条目。
        /// </summary>
        public void Clear()
        {
            dict.Clear();
            sortedSet.Clear();
        }

        /// <summary>
        /// 获取按优先级排序的条目集合（只读遍历）。
        /// </summary>
        /// <returns>按优先级从低到高排序的枚举器</returns>
        public IEnumerable<Entry> GetChunksByPriority()
        {
            return sortedSet;
        }

        /// <summary>
        /// 当前已存储的条目数量。
        /// </summary>
        public int Count => dict.Count;
    }
}
