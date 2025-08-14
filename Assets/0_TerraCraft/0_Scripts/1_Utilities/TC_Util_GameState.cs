using System;
using TC.Core.Define.Class;

namespace TC.Utilities
{
    public static class TC_Util_GameState
    {
        /// <summary>
        /// ���״̬�Ƿ���Ч
        /// </summary>
        public static bool IsValid(TC_Define_Class_EGameState state)
        {
            return Enum.IsDefined(typeof(TC_Define_Class_EGameState), state);
        }

        /// <summary>
        /// �ж��Ƿ���Ҫִ��һ�����߼�
        /// </summary>
        public static bool ShouldRunOnce(TC_Define_Class_EGameState current, TC_Define_Class_EGameState last)
        {
            return current != last;
        }
    }
}
