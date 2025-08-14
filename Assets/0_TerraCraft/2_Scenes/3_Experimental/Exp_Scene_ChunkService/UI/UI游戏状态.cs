using TC.Services;
using TMPro;

namespace Tc.Exp.ChunkService
{
    public class UI游戏状态 : TC.Gameplay.TC_Mono_Base
    {
        public TextMeshProUGUI TMP;

        private void Update()
        {
            TMP.text = $"{TC_Mono_Service_GameState.Instance.CurrentState}";
        }

    }
}


