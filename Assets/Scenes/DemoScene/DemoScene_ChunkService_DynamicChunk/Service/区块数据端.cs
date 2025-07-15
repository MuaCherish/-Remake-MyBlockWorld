using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    public class 区块数据端 : MonoBehaviour
    {

        public Dictionary<Vector3Int, 区块> AllChunks;
        public 跳跃列表 跳表;

        public Material Mat_Chunk;
        public Camera Player_Camera;
        public Transform Player_Transform;


        private void Awake()
        {
            AllChunks = new Dictionary<Vector3Int, 区块>();
            跳表 = new 跳跃列表(Player_Transform);
        }


        void OnDrawGizmos()
        {
            if (AllChunks == null || AllChunks.Count == 0 || Camera.main == null)
                return;

            Gizmos.color = Color.gray;

            Vector3 size = 区块全局设置.区块大小;

            // 先初始化一个空的包围盒，后续扩展它包裹所有区块
            Bounds combinedBounds = new Bounds();

            bool first = true;

            foreach (var kvp in AllChunks)
            {
                Vector3Int logicPos = kvp.Key;
                Vector3 worldPos = 常用数学计算.LogicToWorld(logicPos);
                Bounds bounds = new Bounds(worldPos + size * 0.5f, size);

                if (first)
                {
                    combinedBounds = bounds;
                    first = false;
                }
                else
                {
                    combinedBounds.Encapsulate(bounds);
                }
            }

            // 只画一个大包围盒
            Gizmos.DrawWireCube(combinedBounds.center, combinedBounds.size);
        }



    }


}





