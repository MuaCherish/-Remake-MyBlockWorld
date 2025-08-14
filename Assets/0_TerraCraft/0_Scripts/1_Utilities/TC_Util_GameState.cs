using System;
using TC.Core.Define.Class;

namespace TC.Utilities
{
    public static class TC_Util_GameState
    {
        /// <summary>
        /// 检查状态是否有效
        /// </summary>
        public static bool IsValid(TC_Define_Class_EGameState state)
        {
            return Enum.IsDefined(typeof(TC_Define_Class_EGameState), state);
        }

        /// <summary>
        /// 判断是否需要执行一次性逻辑
        /// </summary>
        public static bool ShouldRunOnce(TC_Define_Class_EGameState current, TC_Define_Class_EGameState last)
        {
            return current != last;
        }
    }
}
