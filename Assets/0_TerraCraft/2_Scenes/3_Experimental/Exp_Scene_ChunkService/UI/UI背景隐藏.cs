using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tc.Exp.ChunkService
{
    public class UI背景隐藏 : TC.Gameplay.TC_Mono_Base
    {
        public Image image;

        public override void Update_GameState_Playing()
        {
            if (image != null)
            {
                Color c = image.color;
                c.a = 0f; // 设置透明度为 0
                image.color = c;
            }
        }
    }
}
