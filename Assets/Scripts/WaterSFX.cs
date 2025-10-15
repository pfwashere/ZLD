using UnityEngine;
using UnityEngine.UI;

public class WaterSFX : MonoBehaviour
{
    private AudioSource waterAudioSource;
    private bool isPlaying = false;
    private AudioClip waterSoundClip;
    public Button toggleButton;

    void Start()
    {
        // สร้าง AudioSource แบบ Dynamic
        waterAudioSource = gameObject.AddComponent<AudioSource>();
        waterAudioSource.clip = waterSoundClip;
        waterAudioSource.loop = true;

        // กำหนดให้ปุ่มเรียก ToggleWaterSound เมื่อกด
        toggleButton.onClick.AddListener(ToggleWaterSound);
    }

    void ToggleWaterSound()
    {
        if (isPlaying)
        {
            waterAudioSource.Stop();
        }
        else
        {
            waterAudioSource.Play();
        }

        isPlaying = !isPlaying;
    }


}
