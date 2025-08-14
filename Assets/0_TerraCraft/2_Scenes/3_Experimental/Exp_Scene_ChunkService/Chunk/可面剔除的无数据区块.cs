using UnityEngine;

namespace Tc.Exp.ChunkService
{
   
    public class 可面剔除的无数据区块
    {
        public Vector3Int myLogicPos;
        public 区块渲染数据 渲染数据 = new 区块渲染数据();
        private 区块数据端 数据端;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logicPos"></param>
        /// <param name="数据端"></param>
        public 可面剔除的无数据区块(Vector3Int logicPos, 区块数据端 _数据端)
        {
            数据端 = _数据端;
            myLogicPos = logicPos;
            渲染数据.chunkMaterial = 数据端.Mat_Chunk;
            

            Caculate();
        } 

        /// <summary>
        /// 开始计算
        /// </summary>
        public void Caculate()
        {
            //chunkMesh = BuildUnitCubeMesh(); // 只生成一次也可以做成静态缓存
            渲染数据.chunkMesh = Mesh对象池.Get();
            BuildUnitCubeMesh(渲染数据.chunkMesh); // 把 mesh 数据写入这个可复用的 mesh


            Vector3 worldPos = 常用数学计算.LogicToWorld(数据端.区块全局数据, myLogicPos);
            Vector3 scale = 数据端.区块全局数据.GetChunkSize();
            渲染数据.matrix = Matrix4x4.TRS(worldPos, Quaternion.identity, scale);

            //计算完成后上传数据
            //还没写

        }

        /// <summary>
        /// 构建一个单位立方体（1x1x1）的 Mesh
        /// </summary>
        private Mesh BuildUnitCubeMesh(Mesh mesh)
        {
            mesh.name = $"UnitCube_{myLogicPos}";

            Vector3[] vertices = new Vector3[8]
            {
                new Vector3(0, 0, 0), // 0
                new Vector3(1, 0, 0), // 1
                new Vector3(1, 1, 0), // 2
                new Vector3(0, 1, 0), // 3
                new Vector3(0, 0, 1), // 4
                new Vector3(1, 0, 1), // 5
                new Vector3(1, 1, 1), // 6
                new Vector3(0, 1, 1)  // 7
            };

            int[] triangles = new int[]
            {
        // Front
        0, 2, 1, 0, 3, 2,
        // Back
        5, 6, 4, 4, 6, 7,
        // Left
        4, 7, 0, 0, 7, 3,
        // Right
        1, 2, 5, 5, 2, 6,
        // Top
        3, 7, 2, 2, 7, 6,
        // Bottom
        0, 1, 4, 4, 1, 5
            };

            // 每个三角面6个顶点，共6面 × 4顶点 = 最多24个唯一顶点（如果要让每面独立贴图）
            // 这里简化，只给8个点一份UV，适合 Unlit 或 Basic 测试材质
            Vector2[] uv = new Vector2[8]
            {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1)
            };

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals(); // 必须有，用于光照
            mesh.RecalculateBounds();

            return mesh;
        }



    }

}
