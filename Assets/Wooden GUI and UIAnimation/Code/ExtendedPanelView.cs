using UnityEngine;
using UnityEngine.UI;
namespace WoodenGUI
{
    public class ExtendedPanelView : MonoBehaviour
    {
        [SerializeField] Button ExpandButton;
        [SerializeField] Transform ExtendedPanel;
        private bool _isExpanded;
        void Start()
        {
            ExpandButton.onClick.AddListener(OnExpandButtonClick);
            ExtendedPanel.gameObject.SetActive(false);
        }

        private void OnExpandButtonClick()
        {
            _isExpanded = !_isExpanded;
            SetExpanded(_isExpanded);
        }

        private void SetExpanded(bool value)
        {
            ExtendedPanel.gameObject.SetActive(value);
            ExpandButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, value ? -90 : 0);
        }
    }
}