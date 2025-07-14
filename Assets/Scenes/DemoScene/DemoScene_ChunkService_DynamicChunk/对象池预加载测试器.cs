using System.Collections;
using UnityEngine;

public class 对象池预加载测试器 : MonoBehaviour
{
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

        Debug.Log($"[测试] Mesh + 区块对象池预加载总耗时: {totalTime * 1000f:F2} ms，" +
                  $"共计帧数: {totalFrames}，平均FPS: {avgFPS:F1}");
    }
}
