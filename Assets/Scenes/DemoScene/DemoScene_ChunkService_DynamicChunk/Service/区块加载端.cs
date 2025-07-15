using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    /// <summary>
    /// 暴露给外部的区块逻辑坐标限制规则
    /// </summary>
    public interface IChunkSpawnRule
    {
        // 是否允许在指定逻辑坐标生成区块
        bool IsValid(Vector3Int logicPos);
    }

    public class 区块加载端 : MonoBehaviour
    {
        public 区块数据端 数据端;
        public 区块对象池预加载器 _区块对象池预加载器;
        public Mesh对象池预加载器 _Mesh对象池预加载器;
        public IChunkSpawnRule 生成规则; // 可以在外部赋值

        private void Update()
        {
            Service_DynamicChunkLoad();
        }

        /// <summary>
        /// 动态加载区块
        /// </summary>
        private void Service_DynamicChunkLoad()
        {
            if (!_区块对象池预加载器.isFinishWarmup || !_Mesh对象池预加载器.isFinishWarmup)
                return;

            // 获取需要生成的区块逻辑坐标列表
            List<Vector3Int> chunksToGenerate = 获取待生成区块逻辑坐标();

            // 提前返回：如果没有要生成的区块，跳过
            if (chunksToGenerate.Count == 0)
                return;

            // 遍历列表，生成区块并添加到 AllChunks 中
            foreach (var logicPos in chunksToGenerate)
            {
                区块 chunk = 区块对象池.Get(logicPos, 数据端);
                数据端.AllChunks.Add(logicPos, chunk);
                //Debug.Log($"[区块加载端] 区块 {logicPos} 已添加到 AllChunks");
            }

            //Debug.Log($"渲染半径为[{区块全局设置.渲染半径}]的首次加载区块数量为[{chunksToGenerate.Count}]");

            // 此处保留清理远距离区块的空间
            // 遍历allchunks，区块的逻辑坐标与玩家的逻辑坐标的距离如果大于渲染半径则卸载区块
        }

        /// <summary>
        /// 获取玩家视野范围内待生成的区块逻辑坐标（排除已存在的）
        /// </summary>
        private List<Vector3Int> 获取待生成区块逻辑坐标()
        {
            List<Vector3Int> result = new List<Vector3Int>();

            Vector3 playerWorldPos = 数据端.Player_Transform.position;
            Vector3Int playerLogicPos = 常用数学计算.WorldToLogic(playerWorldPos);

            int range = 区块全局设置.渲染半径;

            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    for (int z = -range; z <= range; z++)
                    {
                        Vector3Int logicPos = playerLogicPos + new Vector3Int(x, y, z);
                        if (!数据端.AllChunks.ContainsKey(logicPos))
                        {
                            // ✅ 加入规则判断
                            if (生成规则 == null || 生成规则.IsValid(logicPos))
                            {
                                result.Add(logicPos);
                            }
                        }
                    }
                }
            }

            return result;
        }


    }

}

