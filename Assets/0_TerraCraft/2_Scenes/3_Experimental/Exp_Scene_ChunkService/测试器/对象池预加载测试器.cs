using System.Collections;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    public class 对象池预加载测试器 : MonoBehaviour
    {
        public 区块数据端 数据端;
        public Mesh对象池预加载器 mesh预加载器;
        public 区块对象池预加载器 区块预加载器;

        private float startTime;
        private int startFrame;

        private void Start()
        {
            StartCoroutine(测试加载时间());
        }

        private IEnumerator 测试加载时间()
        {
            startTime = Time.realtimeSinceStartup;
            startFrame = Time.frameCount;

            while (mesh预加载器 == null || 区块预加载器 == null)
                yield return null;

            while (!mesh预加载器.isFinishWarmup || !区块预加载器.isFinishWarmup)
            {
                yield return null;
            }

            float totalTime = Time.realtimeSinceStartup - startTime;
            int totalFrames = Time.frameCount - startFrame;
            float avgFPS = totalFrames / totalTime;
            float avgFrameTimeMs = (totalTime / totalFrames) * 1000f;

            Debug.Log($"[测试] Mesh + 区块对象池预加载总耗时: {totalTime:F2} s，" +
                      $"共加载区块数: {常用数学计算.WarmUpMaxChunks(数据端.区块全局数据.逻辑渲染半径)}，" +
                      $"共计帧数: {totalFrames}，平均FPS: {avgFPS:F1}，" +
                      $"平均每帧耗时: {avgFrameTimeMs:F2} ms");
        }

    }

}

