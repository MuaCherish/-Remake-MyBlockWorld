using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tc.Exp.ChunkService
{
    public class UI进度条控制 : TC.Gameplay.TC_Mono_Base
    {
        public Slider slider;
        public 区块对象池预加载器 loader;

        public override void Update_GameState_Loading_Once()
        {
            slider.gameObject.SetActive(true);
        }

        public override void Update_GameState_Loading()
        {
            slider.value = loader.进度条;
        }

        public override void Update_GameState_Playing_Once() 
        {
            slider.gameObject.SetActive(false);
        }
    }


}
