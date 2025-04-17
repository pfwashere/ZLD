using UnityEngine;

public class LogoText : MonoBehaviour
{
    public GameObject tooltip;

    void Start()
    {
        tooltip.SetActive(false);
    }

    void OnMouseOver()
    {
        tooltip.SetActive(true);
    }

    void OnMouseExit()
    {
        tooltip.SetActive(false);
    }

    public void ForceHide()
    {
        tooltip.SetActive(false);
    }
}
