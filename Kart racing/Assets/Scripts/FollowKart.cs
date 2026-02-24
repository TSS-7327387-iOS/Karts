using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowKart : MonoBehaviour
{
    // public Transform target;
    // public Vector3 offset = new Vector3(0f, 5f, -10f);
    // public float followSpeed = 5f;
    //
    // void LateUpdate()
    // {
    //     if (!target) return;
    //
    //     Vector3 desiredPosition = target.position + target.TransformDirection(offset);
    //     transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    //
    //     // Look at the target smoothly
    //     Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, followSpeed * Time.deltaTime);
    // }
    
    public Transform target;
    public float orbitSpeed = 10f;
    public float orbitRadius = 10f;
    public float heightOffset = 3f;
    public float lookSmoothness = 2f;

    private float currentAngle = 0f;

    void LateUpdate()
    {
        if (!target) return;

        // Continuously orbit around the target
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        // Calculate position on a circle around the target
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 orbitPos = new Vector3(
            Mathf.Sin(rad) * orbitRadius,
            heightOffset,
            Mathf.Cos(rad) * orbitRadius
        );

        Vector3 desiredPosition = target.position + orbitPos;
        transform.position = desiredPosition;

        // Look at the kart smoothly
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookSmoothness * Time.deltaTime);
    }
}
