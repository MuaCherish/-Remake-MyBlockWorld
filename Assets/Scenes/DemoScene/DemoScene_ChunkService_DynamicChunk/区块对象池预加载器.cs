using System.Collections;
using UnityEngine;

public class 区块对象池预加载器 : MonoBehaviour
{
    public int 预加载数量 = 200; // 可根据测试机性能调整
    public int 每帧预加载数据量 = 10;
    public 区块数据端 数据端;
    public bool isFinishWarmup = false;

    private void Start()
    {
        StartCoroutine(预加载协程());
    }

    private IEnumerator 预加载协程()
    {
        for (int i = 0; i < 预加载数量; i++)
        {
            Vector3Int fakePos = new Vector3Int(i, 0, 0); // 虚构逻辑坐标，不用加到 AllChunks
            区块 chunk = new 区块(fakePos, 数据端);
            区块对象池.Recycle(chunk); // 丢进池子备用

            if (i % 每帧预加载数据量 == 0)
                yield return null;
        }

        //Debug.Log($"[预加载器] 成功预热 {预加载数量} 个区块对象到池中");
        isFinishWarmup = true;
    }
}