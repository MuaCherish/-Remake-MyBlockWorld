using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTools
{
    [ExecuteAlways]
    public class 摄像机视线调试器 : MonoBehaviour
    {
        public Camera cameraTarget;
        public float 半径 = 100f;
        public int 圆环分段数 = 10;

        private void OnDrawGizmos()
        {
            if (cameraTarget == null)
                return;

            Vector3 camPos = cameraTarget.transform.position;
            Vector3 camForward = cameraTarget.transform.forward;

            // 1. 绘制 XOZ 平面上的圆环
            Gizmos.color = Color.green;
            DrawCircle_XOZ(camPos, 半径, 圆环分段数);

            // 2. 绘制摄像机朝向的射线
            Gizmos.color = Color.red;
            Gizmos.DrawLine(camPos, camPos + camForward * 半径);
        }

        private void DrawCircle_XOZ(Vector3 center, float radius, int segments)
        {
            float angleStep = 360f / segments;
            Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

            for (int i = 1; i <= segments; i++)
            {
                float rad = Mathf.Deg2Rad * (i * angleStep);
                Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }
    }


}

