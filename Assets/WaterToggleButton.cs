using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterToggleButton : MonoBehaviour
{
    public AudioClip waterClip;
    private AudioSource audioSource;
    private bool isPlaying = true;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = waterClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ToggleWaterSound()
    {
        if (isPlaying)
            audioSource.Stop();
        else
            audioSource.Play();

        isPlaying = !isPlaying;
    }

}
