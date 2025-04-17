using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUp : MonoBehaviour
{
    public void StartSlideUp()
    {
        transform.LeanMoveLocal(new Vector2(0, -357), 1).setEaseOutBack();
    }
    public void StartSlideDown()
    {
        transform.LeanMoveLocal(new Vector2(0, -520), 1).setEaseOutBack();
    }
}
