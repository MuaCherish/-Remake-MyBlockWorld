using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    /// <summary>
    /// 一个按与玩家距离排序的 Vector3Int 列表（从近到远）
    /// </summary>
    public class 距离排序表
    {
        private readonly List<Vector3Int> _chunks = new();
        private readonly Transform _playerTransform;

        public 距离排序表(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void Add(Vector3Int chunkCoord)
        {
            float dist = Vector3.SqrMagnitude(常用数学计算.LogicToWorld(chunkCoord) - _playerTransform.position);

            // 找插入位置（可优化为二分）
            int index = _chunks.FindIndex(c =>
                Vector3.SqrMagnitude(常用数学计算.LogicToWorld(c) - _playerTransform.position) > dist);

            if (index < 0)
                _chunks.Add(chunkCoord);
            else
                _chunks.Insert(index, chunkCoord);
        }

        public void Remove(Vector3Int chunkCoord)
        {
            _chunks.Remove(chunkCoord);
        }

        // 遍历全部
        public IEnumerable<Vector3Int> GetAll()
        {
            return _chunks;
        }



        public int Count => _chunks.Count;
    }

}

