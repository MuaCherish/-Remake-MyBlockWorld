using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    /// <summary>
    /// 掌管 Update 的状态基类（所有服务端和实体可继承）
    /// </summary>
    public class MC_Mono_Base : MonoBehaviour
    {
        private GameState lastState = GameState.Start;

        private void Update()
        {
            GameState currentState = MC_Mono_Service_GameState.Instance.CurrentState;

            OnceCheck(currentState);
            TickCheck(currentState);
            Update_GameState_Tick(); // 通用帧逻辑
        }

        private void OnceCheck(GameState currentState) 
        {
            if (currentState != lastState)
            {
                switch (currentState)
                {
                    case GameState.Start:
                        Update_GameState_Start_Once();
                        break;
                    case GameState.Loading:
                        Update_GameState_Loading_Once();
                        break;
                    case GameState.Playing:
                        Update_GameState_Playing_Once();
                        break;
                    case GameState.Pause:
                        Update_GameState_Pause_Once();
                        break;
                }

                lastState = currentState;
            }
        }

        private void TickCheck(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.Start:
                    Update_GameState_Start();
                    break;
                case GameState.Loading:
                    Update_GameState_Loading();
                    break;
                case GameState.Playing:
                    Update_GameState_Playing();
                    break;
                case GameState.Pause:
                    Update_GameState_Pause();
                    break;
            }
        }

        // 可重写的 Update 虚函数接口
        public virtual void Update_GameState_Tick() { }

        public virtual void Update_GameState_Start() { }
        public virtual void Update_GameState_Start_Once() { }

        public virtual void Update_GameState_Loading() { }
        public virtual void Update_GameState_Loading_Once() { }

        public virtual void Update_GameState_Playing() { }
        public virtual void Update_GameState_Playing_Once() { }

        public virtual void Update_GameState_Pause() { }
        public virtual void Update_GameState_Pause_Once() { }
    }

    /// <summary>
    /// 模板 Mono 类，用于演示如何继承 MC_Mono_Base
    /// </summary>
    public class MonoTemplate : MC_Mono_Base
    {
        public override void Update_GameState_Tick()
        {
            Debug.Log("每帧调用（与状态无关）");
        }

        public override void Update_GameState_Start()
        {
            Debug.Log("Start 状态每帧");
        }

        public override void Update_GameState_Start_Once()
        {
            Debug.Log("进入 Start 状态一次性");
        }

        public override void Update_GameState_Loading()
        {
            Debug.Log("Load 状态每帧");
        }

        public override void Update_GameState_Loading_Once()
        {
            Debug.Log("进入 Load 状态一次性");
        }

        public override void Update_GameState_Playing()
        {
            Debug.Log("Play 状态每帧");
        }

        public override void Update_GameState_Playing_Once()
        {
            Debug.Log("进入 Play 状态一次性"); 
        }

        public override void Update_GameState_Pause()
        {
            Debug.Log("Pause 状态每帧");
        }

        public override void Update_GameState_Pause_Once()
        {
            Debug.Log("进入 Pause 状态一次性");
        }
    }
}
