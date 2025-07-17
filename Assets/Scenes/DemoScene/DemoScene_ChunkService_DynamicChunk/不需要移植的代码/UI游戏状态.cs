using DemoScene_ChunkService_DynamicChunk;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    public class UI游戏状态 : MonoBehaviour
    {
        public TextMeshProUGUI TMP;
        public MC_Mono_Service_GameState Service_GameState;

        private void Update()
        {
            TMP.text = $"{Service_GameState.CurrentState}";
        }

    }
}


