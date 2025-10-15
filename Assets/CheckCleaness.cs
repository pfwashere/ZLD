using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckCleaness : MonoBehaviour
{
    public GameObject CleanWaterState1Bar;
    public GameObject CleanWaterState2Bar;
    public GameObject CleanWaterState3Bar;
    public TextMeshPro countdownText;

    private Coroutine countdownRoutine;
    private bool isCounting = false;   // เช็คว่ากำลังจับเวลาอยู่มั้ย
    private string lastWaterTag;       // เก็บชนิดน้ำที่ชนล่าสุด

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterFiltered") ||
            collision.CompareTag("WaterHeated") ||
            collision.CompareTag("WaterClean") ||
            collision.CompareTag("CleanestWater"))
        {
            lastWaterTag = collision.tag;

            // ถ้ายังไม่เคยเริ่มนับ เริ่ม
            if (!isCounting)
            {
                countdownRoutine = StartCoroutine(StartCountdown());
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        isCounting = true;
        float timer = 10f;

        // กระพริบมั่ว ๆ ระหว่างนับถอยหลัง
        while (timer > 0f)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();

            // กระพริบ random
            CleanWaterState1Bar.SetActive(Random.value > 0.5f);
            CleanWaterState2Bar.SetActive(Random.value > 0.5f);
            CleanWaterState3Bar.SetActive(Random.value > 0.5f);

            yield return new WaitForSeconds(0.3f);
            timer -= 0.3f;
        }

        // reset text
        countdownText.text = "";

        // สุดท้ายเปิดค้างตามชนิดน้ำล่าสุด
        if (lastWaterTag == "WaterFiltered" || lastWaterTag == "WaterHeated")
        {
            CleanWaterState1Bar.SetActive(true);
            CleanWaterState2Bar.SetActive(false);
            CleanWaterState3Bar.SetActive(false);
        }
        else if (lastWaterTag == "WaterClean")
        {
            CleanWaterState1Bar.SetActive(true);
            CleanWaterState2Bar.SetActive(true);
            CleanWaterState3Bar.SetActive(false);
        }
        else if (lastWaterTag == "CleanestWater")
        {
            CleanWaterState1Bar.SetActive(true);
            CleanWaterState2Bar.SetActive(true);
            CleanWaterState3Bar.SetActive(true);
        }

        isCounting = false; // จบการนับ
    }
}
