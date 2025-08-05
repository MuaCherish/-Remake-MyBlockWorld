using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_Chunk_MeshGenerate
{
    /// <summary>
    /// 一个面的mesh缓存数据
    /// </summary>
    public class MC_Define_Class_QuadMeshBuffer
    {
        public List<Vector3> vertices;
        public List<int> triangles;
        public List<Vector2> uvs;

        public MC_Define_Class_QuadMeshBuffer() 
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();
        }
    }

}