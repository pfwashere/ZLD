using UnityEngine;
using UnityEngine.UI;

public class Dragout : MonoBehaviour
{
    public GameObject objectToSpawn;         // Prefab to instantiate
    public Canvas canvas;                    // Your canvas
    public Camera uiCamera;                  // Camera rendering the UI (usually the Main Camera or a separate UI Camera)

    public void SpawnAtButton(Button btn)
    {
        // Get button's position in screen space
        Vector3 screenPos = btn.transform.position;

        // Convert screen position to world point
        Vector3 worldPos = uiCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0; // Optional: Flatten z to match 2D space

        // Instantiate the object at the world position
        Instantiate(objectToSpawn, worldPos, Quaternion.identity);
    }
}
