using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class 跳表节点
    {
        public Vector3Int LogicPos;
        public 区块 Chunk;
        public float Distance; // 权重，距离玩家
        public 跳表节点[] Next;

        public 跳表节点(int level, Vector3Int logicPos, 区块 chunk, float distance)
        {
            Next = new 跳表节点[level];
            LogicPos = logicPos;
            Chunk = chunk;
            Distance = distance;
        }
    }

    public class 跳跃列表
    {
        private const float P = 0.5f; // 概率
        private const int MAX_LEVEL = 16;

        private 跳表节点 head;
        private int level;
        private Transform player;

        public 跳跃列表(Transform playerTransform) 
        {
            level = 1;
            player = playerTransform;
            head = new 跳表节点(MAX_LEVEL, new Vector3Int(int.MinValue, int.MinValue, int.MinValue), null, float.MinValue);
        }

        // 计算距离
        private float CalcDistance(Vector3Int logicPos)
        {
            Vector3 worldPos = 常用数学计算.LogicToWorld(logicPos);
            return Vector3.Distance(worldPos, player.position);
        }

        // 生成随机层数
        private int RandomLevel()
        {
            int lvl = 1;
            while (UnityEngine.Random.value < P && lvl < MAX_LEVEL)
                lvl++;
            return lvl;
        }

        // 插入节点
        public void Insert(Vector3Int logicPos, 区块 chunk)
        {
            float distance = CalcDistance(logicPos);
            跳表节点[] update = new 跳表节点[MAX_LEVEL];
            跳表节点 curr = head;

            // 找插入位置
            for (int i = level - 1; i >= 0; i--)
            {
                while (curr.Next[i] != null && curr.Next[i].Distance < distance)
                {
                    curr = curr.Next[i];
                }
                update[i] = curr;
            }

            int newLevel = RandomLevel();
            if (newLevel > level)
            {
                for (int i = level; i < newLevel; i++)
                    update[i] = head;
                level = newLevel;
            }

            var newNode = new 跳表节点(newLevel, logicPos, chunk, distance);
            for (int i = 0; i < newLevel; i++)
            {
                newNode.Next[i] = update[i].Next[i];
                update[i].Next[i] = newNode;
            }
        }

        // 查找节点（按逻辑坐标）
        public 跳表节点 Find(Vector3Int logicPos)
        {
            跳表节点 curr = head;
            for (int i = level - 1; i >= 0; i--)
            {
                while (curr.Next[i] != null && curr.Next[i].LogicPos != logicPos && curr.Next[i].Distance < CalcDistance(logicPos))
                {
                    curr = curr.Next[i];
                }
            }
            curr = curr.Next[0];
            if (curr != null && curr.LogicPos == logicPos)
                return curr;
            return null;
        }

        // 删除节点
        public void Remove(Vector3Int logicPos)
        {
            跳表节点[] update = new 跳表节点[MAX_LEVEL];
            跳表节点 curr = head;

            for (int i = level - 1; i >= 0; i--)
            {
                while (curr.Next[i] != null && curr.Next[i].LogicPos != logicPos && curr.Next[i].Distance < CalcDistance(logicPos))
                {
                    curr = curr.Next[i];
                }
                update[i] = curr;
            }

            curr = curr.Next[0];
            if (curr != null && curr.LogicPos == logicPos)
            {
                for (int i = 0; i < level; i++)
                {
                    if (update[i].Next[i] != curr)
                        break;
                    update[i].Next[i] = curr.Next[i];
                }

                while (level > 1 && head.Next[level - 1] == null)
                    level--;
            }
        }

        // 遍历距离近的区块（例如，获取前N个）
        public List<区块> GetClosestChunks(int count)
        {
            List<区块> list = new List<区块>();
            跳表节点 curr = head.Next[0];
            while (curr != null && list.Count < count)
            {
                list.Add(curr.Chunk);
                curr = curr.Next[0];
            }
            return list;
        }

        // 当玩家移动时，调用此方法更新节点权重（简化：删除旧节点，插入新节点）
        public void UpdateChunkPosition(Vector3Int logicPos, 区块 chunk)
        {
            Remove(logicPos);
            Insert(logicPos, chunk);
        }
    }

}