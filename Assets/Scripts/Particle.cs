using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using vector2 = UnityEngine.Vector2;
using list = System.Collections.Generic.List<Particle>;

using static Config;
using UnityEngine.UI;

using TMPro;
public class Particle : MonoBehaviour
{
    // --- Config ---
    public static int N = Config.N;
    public static float SIM_W = Config.SIM_W;
    public static float BOTTOM = Config.BOTTOM;
    public static float DAM = Config.DAM;
    public static int DAM_BREAK = Config.DAM_BREAK;
    public static float G = Config.G;
    public static float SPACING = Config.SPACING;
    public static float K = Config.K;
    public static float K_NEAR = Config.K_NEAR;
    public static float REST_DENSITY = Config.REST_DENSITY;
    public static float R = Config.R;
    public static float SIGMA = Config.SIGMA;
    public static float MAX_VEL = Config.MAX_VEL;
    public static float WALL_DAMP = Config.WALL_DAMP;
    public static float VEL_DAMP = Config.VEL_DAMP;
    public static float DT = Config.DT;
    public static float WALL_POS = Config.WALL_POS;

    // --- Physics ---
    public vector2 pos;
    public vector2 previous_pos;
    public vector2 visual_pos;
    public float rho = 0.0f;
    public float rho_near = 0.0f;
    public float press = 0.0f;
    public float press_near = 0.0f;
    public list neighbours = new list();
    public vector2 vel = vector2.zero;
    public vector2 force = new vector2(0f, -G);
    public float velocity = 0.0f;
    public int grid_x;
    public int grid_y;

    // --- Visual --- 
    // Filter
    public Color clearWaterColor;  // สีที่อยากให้ใสขึ้น
    private Color originalColor;               // เก็บสีเดิมไว้
    public SpriteRenderer sr;
    private bool isFiltering = false;
    public float fadeSpeed = 5f;
    public GameObject UnfilteredParticle;
    public GameObject UnHeatedParticle;
    public GameObject UnCentrifugedParticle;
    public GameObject UnBufferedParticle;

    //Heating
    public GameObject steamPrefab;
    [Range(0f, 1f)] public float steamChance = 0.07f; // 7% evap

    //Cooling
    public GameObject snowflakePrefab;
    [Range(0f, 1f)] public float snowflakeChance = 0.1f; // 10%

    //Remove some water that might cause too much lag
    [Range(0f, 1f)] public float WaterRemoveChance = 0.05f; // 5%

    //SmellBetter
    public GameObject smellgoodPrefab;
    [Range(0f, 1f)] public float smellBetterChance = 0.4f; // 40% better smell ifkyk


    //--- Tag ---
    //Water > CleanWater1 > CleanWater2 > CleanWater3
    public string WaterFiltered;
    public string WaterHeated;
    public string WaterClean;

    
    //--- ui ---
    public GameObject CleanWaterState1Bar;
    public GameObject CleanWaterState2Bar;
    public GameObject CleanWaterState3Bar;

    public GameObject OneStarUI;   // หน้า/ภาพแสดง 1 ดาว
    public GameObject TwoStarUI;   // หน้า/ภาพแสดง 2 ดาว
    public GameObject ThreeStarUI; // หน้า/ภาพแสดง 3 ดาว

    private bool hasEnded = false;
    private bool OneStar = false;
    private bool TwoStar = false;
    private bool ThreeStar = false;

    void Start()
    {
        // Set initial position
        pos = transform.position;
        previous_pos = pos;
        visual_pos = pos;

        // ดึง SpriteRenderer อัตโนมัติ
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }

        //TagWater
        gameObject.tag = "Water";
        //CleanWaterState1Bar.SetActive(false);
        //CleanWaterState2Bar.SetActive(false);
        //CleanWaterState3Bar.SetActive(false);

        if (OneStarUI) OneStarUI.SetActive(false);
        if (TwoStarUI) TwoStarUI.SetActive(false);
        if (ThreeStarUI) ThreeStarUI.SetActive(false);

    }

    private void Update()
    {
        // ตรวจเฉพาะครั้งแรกที่จบ เพื่อกันทริกเกอร์ซ้ำ
        if (hasEnded) return;

        // ลำดับความสำคัญ: 3 > 2 > 1
        if (ThreeStar) TriggerEnd(3);
        else if (TwoStar) TriggerEnd(2);
        else if (OneStar) TriggerEnd(1);
    }

    private void TriggerEnd(int stars)
    {
        new WaitForSeconds(5f);
        hasEnded = true;

        if (OneStarUI) OneStarUI.SetActive(stars >= 1);
        if (TwoStarUI) TwoStarUI.SetActive(stars >= 2);
        if (ThreeStarUI) ThreeStarUI.SetActive(stars >= 3);

        Time.timeScale = 0f; // Freeze เกม
    }

    //public void SetWaterState(string tag)
    //{
    //    // ปิดทั้งหมดก่อน
    //    CleanWaterState1Bar.SetActive(false);
    //    CleanWaterState2Bar.SetActive(false);
    //    CleanWaterState3Bar.SetActive(false);

    //    if (tag == "WaterFiltered" || tag == "WaterHeated")
    //    {
    //        CleanWaterState1Bar.SetActive(true);
    //    }
    //    else if (tag == "CleanWater")
    //    {
    //        CleanWaterState1Bar.SetActive(true);
    //        CleanWaterState2Bar.SetActive(true);
    //    }
    //    else if (tag == "CleanestWater")
    //    {
    //        CleanWaterState1Bar.SetActive(true);
    //        CleanWaterState2Bar.SetActive(true);
    //        CleanWaterState3Bar.SetActive(true);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    SetWaterState(other.tag);
    //}

    public void UpdateState()
    {
        previous_pos = pos;
        vel += force * Time.deltaTime * DT;
        pos += vel * Time.deltaTime * DT;

        visual_pos = pos;
        transform.position = visual_pos;

        force = new vector2(0, -G);
        vel = (pos - previous_pos) / Time.deltaTime / DT;
        velocity = vel.magnitude;

        if (velocity > MAX_VEL)
            vel = vel.normalized * MAX_VEL;

        rho = 0.0f;
        rho_near = 0.0f;
        neighbours = new list();

        if (pos.y < BOTTOM && name != "Base_Particle")
            Destroy(gameObject);
    }

    public void CalculatePressure()
    {
        press = K * (rho - REST_DENSITY);
        press_near = K_NEAR * rho_near;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        vector2 normal = collision.contacts[0].normal;
        float vel_normal = Vector2.Dot(vel, normal);
        if (vel_normal > 0) return;

        vector2 vel_tangent = vel - normal * vel_normal;
        vel = vel_tangent - normal * vel_normal * WALL_DAMP;
        pos = collision.contacts[0].point + normal * WALL_POS;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.tag);
        if (collision.CompareTag("Filter"))
        {
            if (!isFiltering) // กันเรียกซ้ำ
            {
                isFiltering = true;
                StartCoroutine(FilterFadeEffect());
            }

            if (gameObject.CompareTag("WaterHeated"))
            {
                gameObject.tag = WaterClean;
            }
            else
            {
                gameObject.tag = WaterFiltered;
            }
        }

        if (collision.CompareTag("Unfilter"))
        {
            isFiltering = false;
            StopCoroutine(FilterFadeEffect()); // หยุด fade ถ้าออกจาก filter
            sr.color = originalColor; // กลับเป็นสีเดิม
        }


        if (collision.CompareTag("Heater"))
        {
            if (Random.value <= steamChance)
            {
                if (gameObject.CompareTag("WaterFiltered"))
                {
                    gameObject.tag = WaterClean;
                }
                else
                {
                    gameObject.tag = WaterHeated;
                }
                // สร้างไอน้ำ
                GameObject steam = Instantiate(steamPrefab, transform.position, Quaternion.identity);
                sr.color = new Color(152f, 194f, 202f, 0.3f); // จาง ๆ

                // เริ่ม fx fade in / fade out
                StartCoroutine(SteamFadeEffect(steam));
            }
            
            sr.color = new Color(152f, 194f, 202f, 0.3f); // จาง ๆ          
            Destroy(UnHeatedParticle);

        }

        if (collision.CompareTag("Cooling"))
        {
                sr.color = new Color(88f, 146f, 243f, 200f);
                gameObject.tag = "WaterClean";

                GameObject snowflake = Instantiate(snowflakePrefab, transform.position, Quaternion.identity);

                // เริ่ม fx fade in / fade out
                StartCoroutine(SnowflakeFadeEffect(snowflake));            
        }

        if (collision.CompareTag("buffer"))
        {
            if (Random.value <= WaterRemoveChance)
            {
                Destroy(gameObject);
            }
            Destroy(UnBufferedParticle);
        }

        if (collision.CompareTag("SmellFIltered"))
        {
            if (Random.value <= smellBetterChance)
            {
                GameObject smellgood = Instantiate(smellgoodPrefab, transform.position, Quaternion.identity);

                StartCoroutine(SmellBetterFadeEffect(smellgood));
            }
        }

        if (collision.CompareTag("Centrifuge"))
        {
            if (gameObject.CompareTag("WaterClean"))
            {
                gameObject.tag = "CleanestWater";
                sr.color = new Color(87f, 160f, 250f, 200f); 
            }
            Destroy(UnCentrifugedParticle);
        }

        if (collision.CompareTag("CheckPipe"))
        {
            if (gameObject.CompareTag("WaterFiltered") || gameObject.CompareTag("WaterHeated"))
            {
                OneStar = true;
                CleanWaterState1Bar.SetActive(true);
            }
                

            if (gameObject.CompareTag("WaterClean"))
            {
                TwoStar = true;
                CleanWaterState1Bar.SetActive(true);
                CleanWaterState2Bar.SetActive(true);
            }
                

            if (gameObject.CompareTag("CleanestWater"))
            {
                ThreeStar = true;
                CleanWaterState1Bar.SetActive(true);
                CleanWaterState2Bar.SetActive(true); CleanWaterState3Bar.SetActive(true);
            }
                


        }
        //{
        //    int newLevel = GetLevelFromTag(gameObject.tag);

            //    if (newLevel > currentLevel && !isBlinking)
            //    {
            //        StartCoroutine(BlinkAndUpgrade(newLevel));
            //    }
            //}


            //int GetLevelFromTag(string tag)
            //{
            //    switch (tag)
            //    {
            //        case "WaterFiltered":
            //        case "WaterHeated":
            //            return 1;
            //        case "WaterClean":
            //            return 2;
            //        case "CleanestWater":
            //            return 3;
            //    }
            //    return 0;
            //}



    }

//    IEnumerator BlinkAndUpgrade(int newLevel)
//{
//    isBlinking = true;
//    float timer = 0f;
//    bool state = false;
//    float duration = 3f;

//    while (timer < duration)
//    {
//        state = !state;

//        if (newLevel == 1)
//        {
//            CleanWaterState1Bar.SetActive(state);
//        }
//        else if (newLevel == 2)
//        {
//            CleanWaterState1Bar.SetActive(state);
//            CleanWaterState2Bar.SetActive(state);
//        }
//        else if (newLevel == 3)
//        {
//            CleanWaterState1Bar.SetActive(state);
//            CleanWaterState2Bar.SetActive(state);
//            CleanWaterState3Bar.SetActive(state);
//        }

//        yield return new WaitForSeconds(0.3f);
//        timer += 0.3f;
//    }

//    // ✅ หลังจากกระพริบครบ → เปิดค้างไว้
//    if (newLevel == 1)
//    {
//        CleanWaterState1Bar.SetActive(true);
//    }
//    else if (newLevel == 2)
//    {
//        CleanWaterState1Bar.SetActive(true);
//        CleanWaterState2Bar.SetActive(true);
//    }
//    else if (newLevel == 3)
//    {
//        CleanWaterState1Bar.SetActive(true);
//        CleanWaterState2Bar.SetActive(true);
//        CleanWaterState3Bar.SetActive(true);
//    }

//    currentLevel = newLevel; // อัพเดทเลเวลปัจจุบัน
//    isBlinking = false;
//}


    private IEnumerator FilterFadeEffect()
    {
        float timer = 0f;
        Color startColor = sr.color;

        while (isFiltering && sr.color != clearWaterColor)
        {
            timer += Time.deltaTime * fadeSpeed;
            sr.color = Color.Lerp(startColor, clearWaterColor, timer);

            yield return null;
        }

        Destroy(UnfilteredParticle);
    }

    private IEnumerator SteamFadeEffect(GameObject steam)
    {
        SpriteRenderer srSteam = steam.GetComponent<SpriteRenderer>();
        if (srSteam == null) yield break;

        float riseSpeed = 0.5f;
        float fadeInTime = 0.3f;
        float fadeOutTime = 0.3f;
        float lifeTime = 0.6f;
        float timer = 0f;

        SetAlpha(srSteam, 0f);

        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            // ลอยขึ้น
            steam.transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            // fade in
            if (timer < fadeInTime)
            {
                SetAlpha(srSteam, Mathf.Lerp(0f, 1f, timer / fadeInTime));
            }
            // fade out
            else if (timer > lifeTime - fadeOutTime)
            {
                float t = (timer - (lifeTime - fadeOutTime)) / fadeOutTime;
                SetAlpha(srSteam, Mathf.Lerp(1f, 0f, t));
            }
            else
            {
                SetAlpha(srSteam, 1f);
            }

            yield return null;
        }

        Destroy(UnHeatedParticle);
        new WaitForSeconds(1f);
        Destroy(steam);
    }

    private IEnumerator SmellBetterFadeEffect(GameObject smellgood)
    {
        SpriteRenderer srSmell = smellgood.GetComponent<SpriteRenderer>();
        if (srSmell == null) yield break;

        float riseSpeed = 0.5f;
        float fadeInTime = 0.3f;
        float fadeOutTime = 0.3f;
        float lifeTime = 0.6f;
        float timer = 0f;

        SetAlpha(srSmell, 0f);

        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            // ลอยขึ้น
            smellgood.transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            // fade in
            if (timer < fadeInTime)
            {
                SetAlpha(srSmell, Mathf.Lerp(0f, 1f, timer / fadeInTime));
            }
            // fade out
            else if (timer > lifeTime - fadeOutTime)
            {
                float t = (timer - (lifeTime - fadeOutTime)) / fadeOutTime;
                SetAlpha(srSmell, Mathf.Lerp(1f, 0f, t));
            }
            else
            {
                SetAlpha(srSmell, 1f);
            }

            yield return null;
        }

    }

    private IEnumerator SnowflakeFadeEffect(GameObject snowflake)
    {
        SpriteRenderer srSnowflake = snowflake.GetComponent<SpriteRenderer>();
        if (srSnowflake == null) yield break;

        float riseSpeed = 0.5f;
        float fadeInTime = 0.3f;
        float fadeOutTime = 0.3f;
        float lifeTime = 0.6f;
        float timer = 0f;

        SetAlpha(srSnowflake, 0f);

        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            // ลอยขึ้น
            snowflake.transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            // fade in
            if (timer < fadeInTime)
            {
                SetAlpha(srSnowflake, Mathf.Lerp(0f, 1f, timer / fadeInTime));
            }
            // fade out
            else if (timer > lifeTime - fadeOutTime)
            {
                float t = (timer - (lifeTime - fadeOutTime)) / fadeOutTime;
                SetAlpha(srSnowflake, Mathf.Lerp(1f, 0f, t));
            }
            else
            {
                SetAlpha(srSnowflake, 1f);
            }

            yield return null;
        }
    }

    private void SetAlpha(SpriteRenderer sr, float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }


}
