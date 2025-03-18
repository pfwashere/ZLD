using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menubuttons : MonoBehaviour
{
    public GameObject PausePanel;

    void Update()
    {

    }

    public void Resume_button()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}