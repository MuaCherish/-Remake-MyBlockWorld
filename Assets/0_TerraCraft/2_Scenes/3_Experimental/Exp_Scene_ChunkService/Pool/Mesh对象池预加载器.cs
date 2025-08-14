using System.Collections;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public class Mesh对象池预加载器 : TC.Gameplay.TC_Mono_Base
    {
        public 区块数据端 数据端;
        [ReadOnly] public int 预加载数量 = 200; 
        public int 每帧加载数量 = 100;
        public bool isFinishWarmup = false;

        public override void Update_GameState_Loading_Once()
        {
            StartCoroutine(预加载协程());
        }

        private IEnumerator 预加载协程()
        {
            int 预加载数量 = 常用数学计算.WarmUpMaxChunks(数据端.区块全局数据.逻辑渲染半径);

            for (int i = 0; i < 预加载数量; i++)
            {
                Mesh mesh = new Mesh();
                //mesh.name = $"预加载Mesh_{i}";
                Mesh对象池.Recycle(mesh);

                if (i % 每帧加载数量 == 0)
                    yield return null;
            }

            //Debug.Log($"[Mesh对象池预加载器] 成功预热 {预加载数量} 个 Mesh");
            isFinishWarmup = true;
        }
    }

}

