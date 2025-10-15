using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideButton1 : MonoBehaviour
{
    public void StartSlideUp()
    {
        transform.LeanMoveLocal(new Vector2(0, -371), 1).setEaseOutBack();
    }
    public void StartSlideL()
    {
        transform.LeanMoveLocal(new Vector2(-40, -519), 1).setEaseOutBack();
    }
    public void StartSlideR()
    {
        transform.LeanMoveLocal(new Vector2(40, -519), 1).setEaseOutBack();
    }
}
