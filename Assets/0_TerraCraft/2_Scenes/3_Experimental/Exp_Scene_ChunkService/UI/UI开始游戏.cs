using UnityEngine;
using UnityEngine.UI;

namespace Tc.Exp.ChunkService
{
    /// <summary>
    /// 开始游戏按钮：点击后切换状态为 Loading，并隐藏按钮自身
    /// </summary>
    public class UI开始游戏 : TC.Gameplay.TC_Mono_Base
    {
        public Button button;

        private void Start()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnStartButtonClicked);
            }
            else
            {
                Debug.LogWarning("【UI开始游戏】未绑定 Button！");
            }
        }

        private void OnStartButtonClicked()
        {
            // 切换状态
            TC.Services.TC_Mono_Service_GameState.Instance.SetState(TC.Core.Define.Class.TC_Define_Class_EGameState.Loading);

            // 隐藏按钮 GameObject（可以是只隐藏按钮，也可以是整个界面）
            gameObject.SetActive(false);
        }
    }
}
