using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    public static class MC_Static_区块数据
    {


        public static readonly Vector3[] 顶点序列 = new Vector3[8]
        {
            new Vector3(0.0f, 0.0f, 0.0f), //0
            new Vector3(1.0f, 0.0f, 0.0f), //1
            new Vector3(1.0f, 0.0f, 1.0f), //2
            new Vector3(0.0f, 0.0f, 1.0f), //3
            new Vector3(0.0f, 1.0f, 0.0f), //4
            new Vector3(1.0f, 1.0f, 0.0f), //5
            new Vector3(1.0f, 1.0f, 1.0f), //6
            new Vector3(0.0f, 1.0f, 1.0f), //7
        };

        public static readonly int[,] 索引序列 = new int[6, 4]
        {
            {2, 3, 7, 6}, //Front{2, 3, 7, 7, 6, 2}
            {0, 1, 5, 4}, //Back{0, 1, 5, 5, 4, 0}
            {3, 0, 4, 7}, //Left{3, 0, 4, 4, 7, 3}
            {1, 2, 6, 5}, //Right{1, 2, 6, 6, 5, 1}
            {4, 5, 6, 7}, //Top{4, 5, 6, 6, 7, 4}
            {3, 2, 1, 0}, //Buttom{3, 2, 1, 1, 0, 3}
        };

        public static readonly Vector2[,] UV序列 = new Vector2[6, 4]
        {
            { new Vector2(1,0), new Vector2(0,0), new Vector2(0,1), new Vector2(1,1) }, // Front
            { new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1) }, // Back
            { new Vector2(1,0), new Vector2(0,0), new Vector2(0,1), new Vector2(1,1) }, // Left
            { new Vector2(1,0), new Vector2(0,0), new Vector2(0,1), new Vector2(1,1) }, // Right
            { new Vector2(0,1), new Vector2(1,1), new Vector2(1,0), new Vector2(0,0) }, // Top
            { new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1) }, // Bottom
        };

        public static readonly Vector3Int[] 面朝方向 = new Vector3Int[6]
        {
            new Vector3Int( 0,  0,  1),   //Front
            new Vector3Int( 0,  0, -1),   //Back
            new Vector3Int(-1,  0,  0),   //Left
            new Vector3Int( 1,  0,  0),   //Right
            new Vector3Int( 0,  1,  0),   //Top
            new Vector3Int( 0, -1,  0),   //Buttom
        };

        public static void GetQuad(Orientation orientation, out List<Vector3> vertices, out List<int> triangles, out List<Vector2> uvs)
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();

            int faceIndex = (int)orientation;

            // 取出该方向的 4 个顶点索引
            for (int i = 0; i < 4; i++)
            {
                int vertIndex = 索引序列[faceIndex, i];
                vertices.Add(顶点序列[vertIndex]);
                uvs.Add(UV序列[faceIndex, i]);
            }

            // 添加两个三角形，顶点索引按顺时针
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);

            triangles.Add(2);
            triangles.Add(3);
            triangles.Add(0);
        }

    }

}
