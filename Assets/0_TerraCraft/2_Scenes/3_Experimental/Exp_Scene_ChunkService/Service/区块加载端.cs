using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    /// <summary>
    /// 暴露给外部的区块逻辑坐标限制规则
    /// </summary>
    public interface IChunk_Updater_Rule
    {
        // 是否允许在指定逻辑坐标生成区块
        bool IsValid(Vector3Int logicPos);

    }

    public class 区块加载端 : TC.Gameplay.TC_Mono_Base
    {
        public 区块数据端 数据端;
        public 区块对象池预加载器 _区块对象池预加载器;
        public Mesh对象池预加载器 _Mesh对象池预加载器;

        private void Start()
        {
            //不能使用Start动态加载，因为执行第一次的时候未预热导致函数会直接跳过
        }

        public override void Update_GameState_Playing()
        {
            //提前返回-禁用Update
            if (!AllowUpdate)
                return;

            //提前返回-没有预热完成
            if (!CheckWarmUp())
                return;

            Service_DynamicChunkLoad();
        }


        /// <summary>
        /// 动态加载区块
        /// </summary>
        public void Service_DynamicChunkLoad()
        {

            // 获取需要生成的区块逻辑坐标列表
            List<Vector3Int> chunksToGenerate = 获取待生成区块逻辑坐标();

            if (chunksToGenerate.Count == 0)
                return;

            // 优先级字典支持批量添加
            数据端.AllChunks.AddRange(chunksToGenerate);
            数据端.AllChunks.RemoveChunksBeyondRange();
            数据端.AllChunks.UpdateSortedCache();
            
            //Debug.Log($"渲染半径为[{数据端.区块全局数据.渲染半径}]的首次加载区块数量为[{chunksToGenerate.Count}]");

            // 此处保留清理远距离区块的空间
            // 你可以使用 AllChunks.GetAllChunks() 遍历所有区块来检测远距离区块，调用 Remove 卸载
        }


        /// <summary>
        /// 获取玩家视野范围内待生成的区块逻辑坐标（排除已存在的）
        /// </summary>
        private List<Vector3Int> 获取待生成区块逻辑坐标()
        {
            List<Vector3Int> result = new List<Vector3Int>();

            Vector3 playerWorldPos = 数据端.Player_Transform.position;
            Vector3Int playerLogicPos = 常用数学计算.WorldToLogic(数据端.区块全局数据, playerWorldPos);

            int range = 数据端.区块全局数据.逻辑渲染半径;

            // 限定 Y 方向在上下 3 层（共 7 层）
            int yMin = playerLogicPos.y - 2;
            int yMax = playerLogicPos.y + 1;

            for (int x = -range; x <= range; x++)
            {
                for (int y = yMin - playerLogicPos.y; y <= yMax - playerLogicPos.y; y++)
                {
                    for (int z = -range; z <= range; z++)
                    {
                        Vector3Int logicPos = playerLogicPos + new Vector3Int(x, y, z);

                        if (!数据端.AllChunks.Contains(logicPos))
                        {
                            // 正常欧几里得距离判断
                            if ((logicPos - playerLogicPos).magnitude <= range)
                            {
                                if (生成规则 == null || 生成规则.IsValid(logicPos))
                                {
                                    result.Add(logicPos);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 是否预热完成
        /// </summary>
        /// <returns></returns>
        public bool CheckWarmUp()
        {
            if (_区块对象池预加载器.isFinishWarmup && _Mesh对象池预加载器.isFinishWarmup)
                return true;
            else
                return false;
        }

        #region 调试接口

        public IChunk_Updater_Rule 生成规则; // 可以在外部赋值

        private bool AllowUpdate = true;
        
        //禁用Update
        public void SetAutoUpdateValue(bool _value)
        {
            AllowUpdate = _value;
        }

        #endregion
    }

}

