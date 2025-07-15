using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    public static class 区块全局设置
    {
        public static Vector3Int 区块大小 = new Vector3Int(16, 64, 16);
        public static int 渲染半径 = 3; //即3*16=48米
    } 

    public class 区块渲染数据
    {
        public Mesh chunkMesh;
        public Material chunkMaterial;
        public Matrix4x4 matrix;

        // 默认构造函数，初始化成员为默认值
        public 区块渲染数据()
        {
            chunkMesh = null;
            chunkMaterial = null;
            matrix = Matrix4x4.identity;
        }

        // 带参数的构造函数
        public 区块渲染数据(Mesh mesh, Material mat, Matrix4x4 mat4x4)
        {
            this.chunkMesh = mesh;
            this.chunkMaterial = mat;
            this.matrix = mat4x4;
        }
    }

}
