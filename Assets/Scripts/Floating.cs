using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatSpeed = 1.0f;      // ความเร็วการลอย
    public float floatAmplitude = 0.5f;  // ระยะการลอยขึ้น-ลง

    private Vector2 startPosition;

    void Start()
    {
        // เก็บตำแหน่งเริ่มต้น
        startPosition = transform.position;
    }

    void Update()
    {
        // คำนวณตำแหน่งใหม่ด้วย sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // อัปเดตตำแหน่งใน 2D (x, y)
        transform.position = new Vector2(startPosition.x, newY);
    }
}
