using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUp1 : MonoBehaviour
{
    public void StartSlideUp()
    {
        transform.LeanMoveLocal(new Vector2(0, -456), 1).setEaseOutBack();
    }
    public void StartSlideDown()
    {
        transform.LeanMoveLocal(new Vector2(0, -604), 1).setEaseOutBack();
    }
}
