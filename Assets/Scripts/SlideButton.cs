using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideButton : MonoBehaviour
{
    public void StartSlideUp()
    {
        transform.LeanMoveLocal(new Vector2(0, -275), 1).setEaseOutBack();
    }
    public void StartSlideL()
    {
        transform.LeanMoveLocal(new Vector2(-40, -435), 1).setEaseOutBack();
    }
    public void StartSlideR()
    {
        transform.LeanMoveLocal(new Vector2(40, -435), 1).setEaseOutBack();
    }
}
