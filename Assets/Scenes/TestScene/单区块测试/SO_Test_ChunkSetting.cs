using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC_Test.RenderOneChunk
{
    [CreateAssetMenu(fileName = "Test_ChunkSetting", menuName = "ScriptableObject/Test/ChunkSetting")]
    public class SO_Test_ChunkSetting : ScriptableObject
    {
        //[Foldout("Setting", true)]
        [Header("区块大小")] public Vector3 ChunkSize = new Vector3(16f, 16f, 16f);

        [Header("区块材质")] public Material ChunkMat;
    }
}
