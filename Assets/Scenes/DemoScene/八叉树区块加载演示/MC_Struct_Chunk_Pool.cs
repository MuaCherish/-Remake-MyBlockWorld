using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC_Test
{
    public class MC_Struct_Chunk_Pool : MonoBehaviour
    {
        public static MC_Struct_Chunk_Pool Instance { get; private set; }

        private GameObject chunkPrefab;
        private Transform poolRoot;
        private Queue<GameObject> pool = new Queue<GameObject>();
        private Dictionary<Vector3Int, GameObject> activeChunks = new Dictionary<Vector3Int, GameObject>();
        private int poolSize = 0;
        private bool isInitialized = false;

        public bool IsInitialized => isInitialized;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void InitPool(int renderRange, GameObject chunkPrefeb, Transform root)
        {
            chunkPrefab = chunkPrefeb;
            poolRoot = root;

            poolSize = renderRange * renderRange * 8;
            for (int i = 0; i < poolSize; i++)
            {
                GameObject chunk = Instantiate(chunkPrefab, poolRoot);
                chunk.SetActive(false);
                pool.Enqueue(chunk);
            }
            isInitialized = true;
        }

        /// <summary>
        /// 注册一个区块，分配并放置到目标位置
        /// </summary>
        public GameObject RegisterChunk(Vector3Int chunkCoord)
        {
            if (activeChunks.ContainsKey(chunkCoord))
            {
                Debug.LogWarning($"Chunk {chunkCoord} 已注册，重复注册被忽略");
                return activeChunks[chunkCoord];
            }

            GameObject chunk = GetChunk();
            chunk.transform.position = chunkCoord * 16;
            activeChunks.Add(chunkCoord, chunk);
            return chunk;
        }

        /// <summary>
        /// 注销一个区块，回收到对象池
        /// </summary>
        public void UnregisterChunk(Vector3Int chunkCoord)
        {
            if (!activeChunks.TryGetValue(chunkCoord, out var chunk))
            {
                Debug.LogWarning($"Chunk {chunkCoord} 不存在，无法注销");
                return;
            }

            ReturnChunk(chunk);
            activeChunks.Remove(chunkCoord);
        }

        /// <summary>
        /// 查询某个Chunk是否已激活
        /// </summary>
        public bool IsChunkActive(Vector3Int chunkCoord)
        {
            return activeChunks.ContainsKey(chunkCoord);
        }

        /// <summary>
        /// 获取某个Chunk对象（如果已注册）
        /// </summary>
        public GameObject GetChunkAt(Vector3Int chunkCoord)
        {
            activeChunks.TryGetValue(chunkCoord, out var chunk);
            return chunk;
        }

        /// <summary>
        /// 获取一个空闲Chunk对象（从池中）
        /// </summary>
        private GameObject GetChunk()
        {
            if (pool.Count > 0)
            {
                GameObject chunk = pool.Dequeue();
                chunk.SetActive(true);
                return chunk;
            }
            else
            {
                Debug.LogWarning("对象池耗尽，动态创建新Chunk！");
                return Instantiate(chunkPrefab, poolRoot);
            }
        }

        /// <summary>
        /// 回收Chunk对象
        /// </summary>
        private void ReturnChunk(GameObject chunk)
        {
            chunk.SetActive(false);
            chunk.transform.SetParent(poolRoot);
            pool.Enqueue(chunk);
        }
    }

}

