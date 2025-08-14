using UnityEngine;

namespace TC.Mono.Debugger
{
    /// <summary>
    /// 调试用自由相机控制器（飞行模式）
    /// 放置在4_Debug/TC_Mono_Debugger目录
    /// </summary>
    public class TC_Mono_Debugger_CameraFree : MonoBehaviour
    {
        [Header("移动参数")]
        public float moveSpeed = 10f;         // 基础移动速度
        public float shiftMultiplier = 2f;    // Shift 加速倍率
        public float ctrlMultiplier = 0.3f;   // Ctrl 减速倍率
        public float verticalSpeed = 5f;      // 上下移动速度

        [Header("旋转参数")]
        public float lookSensitivity = 2f;    // 鼠标灵敏度
        public bool lockCursorOnStart = false;// 是否在开始时锁定鼠标

        private float yaw;   // 水平角度
        private float pitch; // 垂直角度
        private bool cursorLocked;

        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;

            LockCursor(lockCursorOnStart);
        }

        void Update()
        {
            HandleCursorToggle();

            if (cursorLocked)
                RotateCamera();

            MoveCamera();
        }

        /// <summary>
        /// 鼠标锁定/解锁切换
        /// </summary>
        private void HandleCursorToggle()
        {
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                LockCursor(!cursorLocked);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                LockCursor(false);
            }
        }

        /// <summary>
        /// 控制相机旋转
        /// </summary>
        private void RotateCamera()
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        /// <summary>
        /// 控制相机移动
        /// </summary>
        private void MoveCamera()
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction += transform.forward;
            if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
            if (Input.GetKey(KeyCode.A)) direction -= transform.right;
            if (Input.GetKey(KeyCode.D)) direction += transform.right;
            if (Input.GetKey(KeyCode.E)) direction += Vector3.up;
            if (Input.GetKey(KeyCode.Q)) direction -= Vector3.up;

            float finalSpeed = moveSpeed;

            // Shift加速，Ctrl减速
            if (Input.GetKey(KeyCode.LeftShift))
                finalSpeed *= shiftMultiplier;
            else if (Input.GetKey(KeyCode.LeftControl))
                finalSpeed *= ctrlMultiplier;

            transform.position += direction.normalized * finalSpeed * Time.deltaTime;
        }

        /// <summary>
        /// 锁定/解锁鼠标
        /// </summary>
        private void LockCursor(bool isLocked)
        {
            cursorLocked = isLocked;
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isLocked;
        }
    }
}
