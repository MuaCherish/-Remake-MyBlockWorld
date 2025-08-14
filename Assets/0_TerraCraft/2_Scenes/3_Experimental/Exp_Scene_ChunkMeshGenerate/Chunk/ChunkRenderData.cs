using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    /// <summary>
    /// 表示一个区块的渲染数据，用于传递给 Graphics 或 MeshRenderer 系统
    /// </summary>
    public class ChunkRenderData
    {
        public Mesh ChunkMesh { get; private set; } // 区块的网格
        public Material[] ChunkMaterials { get; private set; } // 多个材质，对应子网格
        public Matrix4x4 Matrix { get; private set; } = Matrix4x4.identity; // 世界矩阵

        public bool IsValid => ChunkMesh != null && ChunkMaterials != null && ChunkMaterials.Length > 0;

        public ChunkRenderData() { }

        public void Set(Mesh mesh, Vector3 chunkWorldPos)
        {
            ChunkMesh = mesh;

            if (ChunkMesh != null)
            {
                if (ChunkMaterials != null)
                {
                    ChunkMesh.subMeshCount = ChunkMaterials.Length;
                }

                ChunkMesh.RecalculateNormals();
                ChunkMesh.RecalculateBounds();
            }

            Matrix = Matrix4x4.TRS(chunkWorldPos, Quaternion.identity, Vector3.one);
        }

        /// <summary>
        /// 设置网格数据（不带颜色）
        /// </summary>
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

            ChunkMesh.subMeshCount = ChunkMaterials.Length;
            for (int i = 0; i < ChunkMaterials.Length; i++)
            {
                ChunkMesh.SetTriangles(trianglesForEachSubmesh[i], i);
            }

            ChunkMesh.RecalculateNormals();
            ChunkMesh.RecalculateBounds();

            Matrix = Matrix4x4.TRS(chunkWorldPos, Quaternion.identity, Vector3.one);
        }

        /// <summary>
        /// 设置网格数据（带颜色）
        /// </summary>
        public void Set(Vector3[] vertices, Color[] colors, Vector2[] uv, List<int>[] trianglesForEachSubmesh, Vector3 chunkWorldPos)
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
            ChunkMesh.colors = colors; // ✅ 设置顶点颜色

            ChunkMesh.subMeshCount = ChunkMaterials.Length;
            for (int i = 0; i < ChunkMaterials.Length; i++)
            {
                ChunkMesh.SetTriangles(trianglesForEachSubmesh[i], i);
            }

            ChunkMesh.RecalculateNormals();
            ChunkMesh.RecalculateBounds();

            Matrix = Matrix4x4.TRS(chunkWorldPos, Quaternion.identity, Vector3.one);
        }

        public void SetMaterials(Material[] materials)
        {
            ChunkMaterials = materials;
        }

        public void Clear()
        {
            ChunkMesh = null;
            ChunkMaterials = null;
            Matrix = Matrix4x4.identity;
        }
    }
}
