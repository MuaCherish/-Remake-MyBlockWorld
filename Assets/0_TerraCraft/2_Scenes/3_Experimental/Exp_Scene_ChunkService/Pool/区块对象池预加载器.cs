using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public class 区块对象池预加载器 : TC.Gameplay.TC_Mono_Base
    {
        [ReadOnly] public int 预加载数量 = 200; // 可根据测试机性能调整 
        public int 每帧预加载数据量 = 30;
        public 区块数据端 数据端;
        public bool isFinishWarmup = false;
        [Range(0, 1)] public float 进度条 = 0f;

        public override void Update_GameState_Loading_Once()
        {
            StartCoroutine(预加载协程());
        }


        private IEnumerator 预加载协程()
        {
            int 预加载数量 = 常用数学计算.WarmUpMaxChunks(数据端.区块全局数据.逻辑渲染半径);

            for (int i = 0; i < 预加载数量; i++)
            {
                Vector3Int fakePos = new Vector3Int(i, 0, 0);
                可面剔除的无数据区块 chunk = new 可面剔除的无数据区块(fakePos, 数据端);
                区块对象池.Recycle(chunk);

                // ✅ 更新进度条
                进度条 = (float)(i + 1) / 预加载数量;

                if (i % 每帧预加载数据量 == 0)
                    yield return null;
            }

            TC.Services.TC_Mono_Service_GameState.Instance.SetState(TC.Core.Define.Class.TC_Define_Class_EGameState.Playing);
            isFinishWarmup = true;
        }




    }

}
