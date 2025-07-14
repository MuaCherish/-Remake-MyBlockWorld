using System.Collections;
using UnityEngine;

public class Mesh对象池预加载器 : MonoBehaviour
{
    public int 预加载数量 = 200;
    public int 每帧加载数量 = 10;
    public bool isFinishWarmup = false;

    private void Start()
    {
        StartCoroutine(预加载协程());
    }

    private IEnumerator 预加载协程()
    {
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