using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tc.Exp.ChunkMeshGenerate
{
    public static class MC_Define_Config_ChunkRenderData
    {

        public static readonly Vector3[] �������� = new Vector3[8]
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

        public static readonly int[,] ������������ = new int[6, 4]
        {
            {2, 6, 7, 3}, //Front
            {0, 4, 5, 1}, //Back
            {3, 7, 4, 0}, //Left
            {1, 5, 6, 2}, //Right
            {4, 7, 6, 5}, //Top
            {3, 0, 1, 2}, //Buttom
        };

        public static readonly int[] �������������� = new int[6]{0, 1, 2, 2, 3, 0};

        public static readonly Vector2[] UV���� = new Vector2[4]{new Vector2(0,0), new Vector2(0,1), new Vector2(1,1), new Vector2(1,0) };

        /// <summary>
        /// /����һ�������ε����mesh����
        /// </summary>
        /// <param name="rootRelaPos"></param>
        /// <param name="headIndex"></param>
        /// <param name="orientation"></param>
        /// <param name="quadMeshBuffer"></param>
        public static void GetQuad(Vector3Int rootRelaPos,int headIndex, MC_Define_Config_Orientation.Enum_Orientation orientation, out MC_Define_Class_QuadMeshBuffer quadMeshBuffer)
        {
            quadMeshBuffer = new MC_Define_Class_QuadMeshBuffer();

            int faceIndex = (int)orientation;

            // ȡ���÷���� 4 ����������
            for (int i = 0; i < 4; i++)
            {
                int vertIndex = ������������[faceIndex, i];
                quadMeshBuffer.vertices.Add(rootRelaPos + ��������[vertIndex]);
                quadMeshBuffer.uvs.Add(UV����[i]);
            }

            // ������������Σ�����������˳ʱ��
            for (int i = 0; i < 6; i++)
            {
                quadMeshBuffer.triangles.Add(headIndex + ��������������[i]);
            }
            
        }


        /// <summary>
        /// �������Ⱦ�����ֵ
        /// </summary>
        /// <param name="lightValue">���ȵȼ�</param>
        /// <returns></returns>
        private static readonly Vector2 LightnessRange = new Vector2(0.05f, 0.8f); //���ȷ�Χ
        private static readonly int LightLevelCount = 16;  //���ȵȼ�
        private static readonly float LightStep = (LightnessRange.y - LightnessRange.x) / (LightLevelCount - 1);  //����
        public static Color GetVoxelLight(byte lightValue)
        {
            if (lightValue >= LightLevelCount)
                lightValue = (byte)(LightLevelCount - 1);

            float brightness = LightnessRange.x + LightStep * lightValue;
            return new Color(brightness, brightness, brightness, 1f);
        }






    }

}
