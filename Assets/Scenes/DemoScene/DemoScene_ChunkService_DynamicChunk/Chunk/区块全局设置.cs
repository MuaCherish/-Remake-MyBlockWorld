using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoScene_ChunkService_DynamicChunk
{
    [CreateAssetMenu(fileName = "区块全局设置", menuName = "设置/区块全局设置")]
    public class 区块全局设置 : ScriptableObject
    {
        [Header("区块大小 (单位格)")]
        public Vector3Int 区块大小 = new Vector3Int(16, 128, 16);

        [Header("逻辑渲染半径")]
        [Range(1, 20)]
        public int 渲染半径 = 1;
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
