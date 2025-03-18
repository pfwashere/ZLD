using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop2D : MonoBehaviour
{
    Vector3 offset;
    Collider2D collider2D;
    public string destinationTag = "DropArea";
    public float snapRange = 0.3f; 
    private Transform nearestSlot; 

    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;

        
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation *= Quaternion.Euler(0, 0, 90);
        }
    }

    void OnMouseUp()
    {
        collider2D.enabled = false;
        FindNearestSlot();

        if (nearestSlot != null)
        {
            float distance = Vector3.Distance(transform.position, nearestSlot.position);

            
            if (distance <= snapRange)
            {
                transform.position = nearestSlot.position; 
            }
        }

        collider2D.enabled = true;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    void FindNearestSlot()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag(destinationTag);
        float minDistance = Mathf.Infinity;
        nearestSlot = null;

        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSlot = slot.transform;
            }
        }
    }
}
