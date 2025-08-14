using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkService
{
    [CreateAssetMenu(fileName = "区块全局设置", menuName = "ScriptableObjects/区块全局设置")]
    public class 区块全局设置 : ScriptableObject
    {
        [Range(2, 16)] public int 区块宽度 = 16;
        [Range(2, 256)] public int 区块高度 = 128;
        [Range(2, 16)] public int 逻辑渲染半径 = 2;
        
        public Vector3Int GetChunkSize()
        {
            return new Vector3Int(区块宽度, 区块高度, 区块宽度);
        }
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
