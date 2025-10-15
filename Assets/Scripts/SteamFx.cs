using UnityEngine;

public class SteamFx : MonoBehaviour
{
    public float fadeInTime = 0.5f;   // เวลา fade in
    public float fadeOutTime = 0.5f;  // เวลา fade out
    public float lifeTime = 2f;       // อยู่รวมๆ กี่วิ
    public float riseSpeed = 1f;      // ความเร็วลอยขึ้น

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0f; // เริ่มโปร่งใส
        sr.color = c;
        timer = 0f;
    }

    void Update()
    {

        timer += Time.deltaTime;

        // ลอยขึ้น
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        // ค่อยๆ fade in
        if (timer < fadeInTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            SetAlpha(alpha);
        }
        // ค่อยๆ fade out
        else if (timer > lifeTime - fadeOutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, (timer - (lifeTime - fadeOutTime)) / fadeOutTime);
            SetAlpha(alpha);
        }

        // หมดอายุ
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void SetAlpha(float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}
