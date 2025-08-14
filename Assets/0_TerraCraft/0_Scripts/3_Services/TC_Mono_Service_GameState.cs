using UnityEngine;
using TC.Core.Define.Class; // ���� GameState �� GameStateData

namespace TC.Services
{
    /// <summary>
    /// ��Ϸ״̬����ȫ�ַ���㣩
    /// </summary>
    public class TC_Mono_Service_GameState : MonoBehaviour
    {
        #region ����
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
        /// ��ǰ��Ϸ״̬
        /// </summary>
        public TC_Define_Class_EGameState CurrentState { get; private set; } = TC_Define_Class_EGameState.Start;

        /// <summary>
        /// ״̬���ݣ���ѡ������ LastState �ȣ�
        /// </summary>
        public TC_Define_Class_CGameStateData StateData { get; private set; } = new TC_Define_Class_CGameStateData();

        #region Unity �ص�
        private void Awake()
        {
            // ȷ������Ψһ
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
            // ������Դ���״̬�л��߼������綨ʱ�л���Debug ��ݼ����Ե�
        }
        #endregion

        #region ״̬������
        /// <summary>
        /// ������Ϸ״̬
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
        /// �Ƿ���ָ��״̬
        /// </summary>
        public bool IsState(TC_Define_Class_EGameState state)
        {
            return CurrentState == state;
        }
        #endregion
    }
}
