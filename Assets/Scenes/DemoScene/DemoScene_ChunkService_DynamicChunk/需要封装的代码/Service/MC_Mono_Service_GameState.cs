using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        Start,
        Loading,
        Playing,
        Pause,
    }
     
    /// <summary>
    /// 游戏状态的全局管理器（单例）
    /// </summary>
    public class MC_Mono_Service_GameState : MC_Mono_Base
    {
        public static MC_Mono_Service_GameState Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Start;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// 状态改变
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(GameState newState)
        {
            if (newState != CurrentState)
            {
                //Debug.Log($"[GameState] 状态切换：{CurrentState} → {newState}");
                CurrentState = newState;
            }
        }



    }
}