using UnityEngine;
using System.Collections;

public class LogoTextUG : MonoBehaviour
{
    public CanvasGroup tooltip; // ใช้ CanvasGroup เพื่อทำ Fade
    public float fadeDuration = 0.5f; // ระยะเวลาค่อยๆ fade
    private bool isVisible = false; // ตอนนี้แสดง tooltip อยู่มั้ย
    private bool isFading = false;

    void Start()
    {
        tooltip.alpha = 0f;  // เริ่มต้นโปร่งใส
        tooltip.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (!isFading)
        {
            if (!isVisible)
            {
                StartCoroutine(FadeIn());
            }
            else
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        tooltip.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            tooltip.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        tooltip.alpha = 1f;
        isVisible = true;
        isFading = false;
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            tooltip.alpha = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            yield return null;
        }

        tooltip.alpha = 0f;
        tooltip.gameObject.SetActive(false);
        isVisible = false;
        isFading = false;
    }

    public void ForceHide()
    {
        StopAllCoroutines();
        tooltip.alpha = 0f;
        tooltip.gameObject.SetActive(false);
        isVisible = false;
        isFading = false;
    }
}
