using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    /// <summary>
    /// 表示一个区块的渲染数据，用于传递给 Graphics 或 MeshRenderer 系统
    /// </summary>
    public class ChunkRenderData
    {
        public Mesh ChunkMesh { get; private set; } // 区块的网格
        public Material[] ChunkMaterials { get; private set; } // 多个材质，对应子网格
        public Matrix4x4 Matrix { get; private set; } = Matrix4x4.identity; // 世界矩阵

        /// <summary>
        /// 是否已准备好渲染
        /// </summary>
        public bool IsValid => ChunkMesh != null && ChunkMaterials != null && ChunkMaterials.Length > 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ChunkRenderData() { }

        /// <summary>
        /// 设置渲染数据（不包括材质）
        /// 注意不需要调用Clear()了
        /// </summary>
        public void Set(Mesh mesh, Vector3 chunkWorldPos)
        {
            ChunkMesh = mesh;

            if (ChunkMesh != null)
            {
                // 保证子网格数量和材质数组长度一致
                if (ChunkMaterials != null)
                {
                    ChunkMesh.subMeshCount = ChunkMaterials.Length;
                }

                ChunkMesh.RecalculateNormals();
                ChunkMesh.RecalculateBounds();
            }

            Matrix = Matrix4x4.TRS(chunkWorldPos, Quaternion.identity, Vector3.one);
        }

        public void Set(Vector3[] vertices, Vector2[] uv, List<int>[] trianglesForEachSubmesh, Vector3 chunkWorldPos)
        {
            if (ChunkMesh == null)
            {
                ChunkMesh = new Mesh();
                ChunkMesh.name = "ChunkMesh";
            }
            else
            {
                ChunkMesh.Clear();
            }

            ChunkMesh.vertices = vertices;
            ChunkMesh.uv = uv;

            // 设置子网格数量，必须和材质数量对应
            ChunkMesh.subMeshCount = ChunkMaterials.Length;

            // 逐个设置子网格的三角形索引
            for (int i = 0; i < ChunkMaterials.Length; i++)
            {
                ChunkMesh.SetTriangles(trianglesForEachSubmesh[i], i);
            }

            ChunkMesh.RecalculateNormals();
            ChunkMesh.RecalculateBounds();

            Matrix = Matrix4x4.TRS(chunkWorldPos, Quaternion.identity, Vector3.one);
        }

        /// <summary>
        /// 单独设置材质数组
        /// </summary>
        public void SetMaterials(Material[] materials)
        {
            ChunkMaterials = materials;
        }

        /// <summary>
        /// 清空渲染数据
        /// </summary>
        public void Clear()
        {
            ChunkMesh = null;
            ChunkMaterials = null;
            Matrix = Matrix4x4.identity;
        }
    }
}
