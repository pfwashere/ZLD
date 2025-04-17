using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDescriptionManager : MonoBehaviour
{
    public static ItemDescriptionManager _instance;

    public TextMeshProUGUI textComponent; 


    void Start()
    {
       Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowDescription()
    {
        gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        gameObject?.SetActive(false);
    }
}
