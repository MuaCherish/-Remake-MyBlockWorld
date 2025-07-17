using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemoScene_ChunkService_DynamicChunk
{
    public class UI背景隐藏 : MC_Mono_Base
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
