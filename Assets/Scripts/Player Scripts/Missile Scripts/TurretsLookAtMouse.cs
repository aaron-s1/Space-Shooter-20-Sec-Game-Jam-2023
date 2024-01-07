using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsLookAtMouse : MonoBehaviour
{
    [SerializeField] Transform originPoint;
    [SerializeField] float rotationOffset = 45f;
    [SerializeField] float minDistanceToStopRotation = 1f;


    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 directionToMouse = mousePosition - originPoint.position;
        directionToMouse.z = 0;

        float distanceToMouse = directionToMouse.magnitude;

        //
        // Debug.DrawLine(originPoint.position, originPoint.position + directionToMouse.normalized * minDistanceToStopRotation, Color.green);
        // VisualizeAllowedAngle(directionToMouse);

        float angleToMouse = Vector3.Angle(transform.up, directionToMouse);

        if (distanceToMouse > minDistanceToStopRotation && IsWithinAllowedAngle(angleToMouse))
        {
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        }
    }

    bool IsWithinAllowedAngle(float angle)
    {
        float allowedAngleRange = 90f;
        return Mathf.Abs(angle) <= allowedAngleRange / 2f;
    }

    void VisualizeAllowedAngle(Vector3 directionToMouse)
    {
        Vector3 leftDirection = Quaternion.AngleAxis(-45f, Vector3.forward) * directionToMouse.normalized;
        Vector3 rightDirection = Quaternion.AngleAxis(45f, Vector3.forward) * directionToMouse.normalized;

        Debug.DrawRay(originPoint.position, leftDirection * 2f, Color.red);
        Debug.DrawRay(originPoint.position, rightDirection * 2f, Color.red);
    }
}
