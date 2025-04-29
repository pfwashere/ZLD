using UnityEngine;
namespace UIAnimation.Addon
{
    public class Switch : MonoBehaviour
    {
        // Array of RectTransform objects to switch between
        [SerializeField] RectTransform[] items;

        // Current index of the active object in the array
        private int currentIndex = 0;

        /// <summary>
        /// Switches the active item in the array.
        /// </summary>
        /// <param name="direction">The direction to switch: 1 for next, -1 for previous.</param>
        public void SwitchMethod(int direction)
        {
            if (items == null || items.Length == 0)
            {
                Debug.LogWarning("Items array is empty or null!");
                return;
            }

            // Deactivate the currently active object
            items[currentIndex].gameObject.SetActive(false);

            // Update index and wrap around if necessary
            currentIndex = (currentIndex + direction + items.Length) % items.Length;

            // Activate the new current object
            items[currentIndex].gameObject.SetActive(true);

            // Log the current active item's details
            Debug.Log($"Current Index: {currentIndex}, Object: {items[currentIndex].name}");
        }
    }
}