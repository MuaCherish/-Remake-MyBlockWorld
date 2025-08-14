using UnityEngine;

namespace TC.Mono.Debugger
{
    /// <summary>
    /// 调试用：在场景视图中绘制摄像机朝向射线和地面圆环
    /// </summary>
    [ExecuteAlways]
    public class TC_Mono_Debugger_CameraViewGizmo : MonoBehaviour
    {
        [Header("Target Camera")]
        public Camera cameraTarget;

        [Header("Circle Settings")]
        public float radius = 100f;
        public int segments = 10;

        private void OnDrawGizmos()
        {
            if (cameraTarget == null)
                return;
            
            Vector3 camPos = cameraTarget.transform.position;
            Vector3 camForward = cameraTarget.transform.forward;

            // 1. 绘制 XOZ 平面上的圆环
            Gizmos.color = Color.green;
            DrawCircleXOZ(camPos, radius, segments);

            // 2. 绘制摄像机朝向的射线
            Gizmos.color = Color.red;
            Gizmos.DrawLine(camPos, camPos + camForward * radius);
        }

        /// <summary>
        /// 在 XOZ 平面绘制一个圆环
        /// </summary>
        private void DrawCircleXOZ(Vector3 center, float radius, int segments)
        {
            if (segments < 3) return; // 至少三个点才是圆

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
