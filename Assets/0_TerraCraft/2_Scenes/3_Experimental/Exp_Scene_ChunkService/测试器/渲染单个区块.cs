using UnityEngine;

namespace Tc.Exp.ChunkService
{

    public class ��Ⱦ�������� : MonoBehaviour
    {
        public �������ݶ� ���ݶ�;

        private �����޳������������� chunk;

        private void Start()
        {
            Vector3Int pos = Vector3Int.zero;

            chunk = new �����޳�������������(pos, ���ݶ�);
        }

        private void OnRenderObject()
        {
            if (chunk != null)
            {
                Graphics.DrawMesh(chunk.��Ⱦ����.chunkMesh, chunk.��Ⱦ����.matrix, chunk.��Ⱦ����.chunkMaterial, 0);
            }
        }
    }

}
