using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUp : MonoBehaviour
{
    public void StartSlideUp()
    {
        transform.LeanMoveLocal(new Vector2(0, -470), 1).setEaseOutBack();
    }
    public void StartSlideDown()
    {
        transform.LeanMoveLocal(new Vector2(0, -611), 1).setEaseOutBack();
    }
}
