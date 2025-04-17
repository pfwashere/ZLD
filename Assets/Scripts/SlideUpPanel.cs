using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUpPanel : MonoBehaviour
{
    public void StartPanelSlideUp()
    {
        transform.LeanMoveLocal(new Vector2(10, 0), 1);
    }
}
