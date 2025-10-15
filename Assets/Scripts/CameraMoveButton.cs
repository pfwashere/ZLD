using UnityEngine;

public class CameraMoveButton : MonoBehaviour
{
    public Camera mainCamera;       // กล้องหลัก
    public Vector3 moveOffset;      // ระยะที่จะเลื่อนต่อการคลิก
    public float smoothTime = 0.3f; // เวลาที่ใช้เลื่อนไปถึงจุดหมาย

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        targetPosition = mainCamera.transform.position;
    }

    public void MoveCamera()
    {
        targetPosition += moveOffset; // เปลี่ยนเป้าหมายทันทีที่กด
    }

    void Update()
    {
        // เคลื่อนที่ไปหาเป้าหมายแบบ Smooth และไม่สั่น
        mainCamera.transform.position = Vector3.SmoothDamp(
            mainCamera.transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
