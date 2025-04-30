using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timer = 0f;
    public TextMeshProUGUI timerText; // Drag your TMP text here via Inspector

    // Update is called once per frame
    void Update()
    {
        // คำนวณเวลา
        timer += Time.deltaTime;

        // แปลงเวลาเป็น นาที : วินาที
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        // อัปเดต UI
        if (timerText != null)
        {
            timerText.text = string.Format("Time Counter {0:00}:{1:00}", minutes, seconds);
        }
    }
}
