using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public static class 常用数学计算
    {
        public static Vector3 LogicToWorld(区块全局设置 chunkSetting, Vector3Int logicPos)
        {
            return new Vector3(logicPos.x * chunkSetting.区块宽度, logicPos.y * chunkSetting.区块高度, logicPos.z * chunkSetting.区块宽度);
        }

        public static Vector3Int WorldToLogic(区块全局设置 chunkSetting, Vector3 relaPos)
        {
            return new Vector3Int((int)relaPos.x / chunkSetting.区块宽度, (int)relaPos.y / chunkSetting.区块高度, (int)relaPos.z / chunkSetting.区块宽度);
        }

        /// <summary>
        /// 预热的最大区块数量计算公式
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int WarmUpMaxChunks(int n)
        {
            float result = 0.3333f * n * n * n + 24.8571f * n * n - 47.4762f * n + 28.6f;
            return Mathf.CeilToInt(result);
        }

    }

}
