using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsLookAtMouse : MonoBehaviour
{
    public Transform originPoint; // Assign the origin point in the Inspector
    public float rotationOffset = 45f; // Adjust this offset as needed

    void Update()
    {
        // Get the mouse position in the world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the origin point to the mouse position
        Vector3 directionToMouse = mousePosition - originPoint.position;
        directionToMouse.z = 0; // Ensure the direction is in the 2D plane

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Rotate the turret to face the mouse with the specified offset
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }
}
