using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKartMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Movement Settings")]
    public float maxSpeed = 15f;
    public float acceleration = 10f;
    public float turnSpeed = 3f;
    public float brakingForce = 5f;
    public float waypointReachDistance = 3f;

    private Rigidbody rb;
    private int currentWaypointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z); // Keep movement on XZ plane

        // Rotate towards target
        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

        // Adjust speed based on turn angle
        float angleToTarget = Vector3.Angle(transform.forward, flatDirection);
        float speedFactor = Mathf.Clamp01(1f - (angleToTarget / 90f)); // Reduce speed when turning hard
        float targetSpeed = maxSpeed * speedFactor;

        // Accelerate towards target speed
        Vector3 forwardVelocity = transform.forward * targetSpeed;
        Vector3 currentVelocity = rb.velocity;
        Vector3 velocityChange = forwardVelocity - currentVelocity;
        velocityChange.y = 0; // Don't modify Y (gravity)
        rb.AddForce(velocityChange.normalized * acceleration, ForceMode.Acceleration);

        // Slow down if very close to waypoint
        if (Vector3.Distance(transform.position, target.position) < waypointReachDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
