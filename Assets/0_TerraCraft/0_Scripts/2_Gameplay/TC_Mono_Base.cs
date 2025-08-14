using UnityEngine;
using TC.Core.Define.Class;
using TC.Utilities;
using TC.Services; // ���� TC_Mono_Service_GameState �� Services ��

namespace TC.Gameplay
{
    /// <summary>
    /// MonoBehaviour ״̬���ࣨ����˺�ʵ��ɼ̳У�
    /// </summary>
    public class TC_Mono_Base : MonoBehaviour
    {
        private TC_Define_Class_CGameStateData stateData = new TC_Define_Class_CGameStateData();

        private void Update()
        {
            // ��ȫ�жϣ�GameState ���񲻴���ʱֱ�ӷ���
            if (TC_Mono_Service_GameState.Instance == null)
                return;

            stateData.CurrentState = TC_Mono_Service_GameState.Instance.CurrentState;

            // ��ȫ�жϣ�CurrentState δ��ʼ��ʱֱ�ӷ���
            if (!TC_Util_GameState.IsValid(stateData.CurrentState))
                return;

            OnceCheck(stateData.CurrentState);
            TickCheck(stateData.CurrentState);
            Update_GameState_Tick(); // ͨ��֡��s��
        }

        private void OnceCheck(TC_Define_Class_EGameState currentState)
        {
            if (TC_Util_GameState.ShouldRunOnce(currentState, stateData.LastState))
            {
                switch (currentState)
                {
                    case TC_Define_Class_EGameState.Start:
                        Update_GameState_Start_Once();
                        break;
                    case TC_Define_Class_EGameState.Loading:
                        Update_GameState_Loading_Once();
                        break;
                    case TC_Define_Class_EGameState.Playing:
                        Update_GameState_Playing_Once();
                        break;
                    case TC_Define_Class_EGameState.Pause:
                        Update_GameState_Pause_Once();
                        break;
                }

                stateData.LastState = currentState;
            }
        }

        private void TickCheck(TC_Define_Class_EGameState currentState)
        {
            switch (currentState)
            {
                case TC_Define_Class_EGameState.Start:
                    Update_GameState_Start();
                    break;
                case TC_Define_Class_EGameState.Loading:
                    Update_GameState_Loading();
                    break;
                case TC_Define_Class_EGameState.Playing:
                    Update_GameState_Playing();
                    break;
                case TC_Define_Class_EGameState.Pause:
                    Update_GameState_Pause();
                    break;
            }
        }

        // ����д�� Update �麯���ӿ�
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
}
