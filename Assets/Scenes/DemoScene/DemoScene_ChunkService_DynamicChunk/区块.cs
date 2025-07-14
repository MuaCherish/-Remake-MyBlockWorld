using UnityEngine;

public class 区块
{
    public Vector3Int myLogicPos;

    // Mesh
    public Mesh chunkMesh;
    public Matrix4x4 matrix;
    public Material chunkMaterial;

    public 区块(Vector3Int logicPos, 区块数据端 数据端)
    {
        myLogicPos = logicPos;
        chunkMaterial = 数据端.Mat_Chunk;

        Caculate();

        数据端.ComputeReadyChunks.Add(myLogicPos);
    }

    public void Caculate()
    {
        //chunkMesh = BuildUnitCubeMesh(); // 只生成一次也可以做成静态缓存
        chunkMesh = Mesh对象池.Get();
        BuildUnitCubeMesh(chunkMesh); // 把 mesh 数据写入这个可复用的 mesh


        Vector3 worldPos = 常用数学计算.LogicToWorld(myLogicPos);
        Vector3 scale = 区块全局设置.区块大小;
        matrix = Matrix4x4.TRS(worldPos, Quaternion.identity, scale);
    }

    /// <summary>
    /// 构建一个单位立方体（1x1x1）的 Mesh
    /// </summary>
    private Mesh BuildUnitCubeMesh(Mesh mesh)
    {
    
        mesh.name = $"UnitCube_{myLogicPos}";

        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1)
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
