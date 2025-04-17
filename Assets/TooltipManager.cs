using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    public GameObject tooltipObject;
    public Text tooltipText;

    void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string message, Vector3 position)
    {
        tooltipObject.SetActive(true);
        tooltipObject.transform.position = position;
        tooltipText.text = message;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
