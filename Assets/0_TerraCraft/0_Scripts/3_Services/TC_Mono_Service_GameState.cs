using UnityEngine;
using TC.Core.Define.Class; // 引用 GameState 和 GameStateData

namespace TC.Services
{
    /// <summary>
    /// 游戏状态服务（全局服务层）
    /// </summary>
    public class TC_Mono_Service_GameState : MonoBehaviour
    {
        #region 单例
        private static TC_Mono_Service_GameState _instance;
        public static TC_Mono_Service_GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TC_Mono_Service_GameState>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("TC_Mono_Service_GameState");
                        _instance = go.AddComponent<TC_Mono_Service_GameState>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 当前游戏状态
        /// </summary>
        public TC_Define_Class_EGameState CurrentState { get; private set; } = TC_Define_Class_EGameState.Start;

        /// <summary>
        /// 状态数据（可选，保存 LastState 等）
        /// </summary>
        public TC_Define_Class_CGameStateData StateData { get; private set; } = new TC_Define_Class_CGameStateData();

        #region Unity 回调
        private void Awake()
        {
            // 确保单例唯一
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetState(TC_Define_Class_EGameState.Start);
        }

        private void Update()
        {
            // 这里可以处理状态切换逻辑，比如定时切换、Debug 快捷键测试等
        }
        #endregion

        #region 状态管理方法
        /// <summary>
        /// 设置游戏状态
        /// </summary>
        public void SetState(TC_Define_Class_EGameState newState)
        {
            if (!System.Enum.IsDefined(typeof(TC_Define_Class_EGameState), newState))
                return;

            StateData.LastState = CurrentState;
            CurrentState = newState;
            StateData.CurrentState = newState;
        }

        /// <summary>
        /// 是否处于指定状态
        /// </summary>
        public bool IsState(TC_Define_Class_EGameState state)
        {
            return CurrentState == state;
        }
        #endregion
    }
}
