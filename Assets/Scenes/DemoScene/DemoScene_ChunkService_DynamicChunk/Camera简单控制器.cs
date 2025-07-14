using UnityEngine;

public class Camera简单控制器 : MonoBehaviour
{
    public float moveSpeed = 10f;          // 默认移动速度
    public float shiftMultiplier = 2f;     // Shift 加速倍率
    public float ctrlMultiplier = 0.3f;    // Ctrl 减速倍率
    public float lookSpeed = 2f;           // 鼠标灵敏度
    public float upDownSpeed = 5f;         // 上升/下降速度

    private float yaw = 0f;
    private float pitch = 0f;

    private bool cursorLocked = false;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        LockCursor(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) LockCursor(true);
        if (Input.GetKeyDown(KeyCode.Escape)) LockCursor(false);

        if (cursorLocked)
        {
            RotateCamera();
        }

        MoveCamera();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void MoveCamera()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += transform.forward;
        if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
        if (Input.GetKey(KeyCode.A)) direction -= transform.right;
        if (Input.GetKey(KeyCode.D)) direction += transform.right;
        if (Input.GetKey(KeyCode.E)) direction += transform.up;     // 上升
        if (Input.GetKey(KeyCode.Q)) direction -= transform.up;     // 下降

        float finalSpeed = moveSpeed;

        // 优先Shift加速，其次Ctrl减速
        if (Input.GetKey(KeyCode.CapsLock))
        {
            finalSpeed *= shiftMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            finalSpeed *= ctrlMultiplier;
        }

        transform.position += direction.normalized * finalSpeed * Time.deltaTime;
    }

    void LockCursor(bool isLocked)
    {
        cursorLocked = isLocked;
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}
