using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Follow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;  // The GameObject the camera should follow

    [Header("Rotation Settings")]
    [Tooltip("Time (in seconds) for the camera to smooth its rotation.")]
    public float rotationSmoothTime = 0.2f;
    [Tooltip("Minimum angle difference before the camera starts rotating.")]
    public float deadzoneAngle = 5f;

    [Header("Position Settings")]
    [Tooltip("Time (in seconds) for the camera to smooth its position movement.")]
    public float positionSmoothTime = 0.5f;
    [Tooltip("How far the camera should maintain from the target on Z axis.")]
    public float offsetZ = 0f;

    private Vector3 currentVelocity;
    private float initialX;  // Store initial X position
    private float initialY;  // Store initial Y position

    void Start()
    {
        // Store the initial X and Y positions
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Handle position following (Z axis only)
        Vector3 targetPosition = transform.position;
        targetPosition.x = initialX;  // Lock X position
        targetPosition.y = initialY;  // Lock Y position
        targetPosition.z = target.position.z + offsetZ;

        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref currentVelocity, 
            positionSmoothTime
        );

        // Calculate the direction from the camera to the target.
        Vector3 directionToTarget = target.position - transform.position;
        if (directionToTarget.sqrMagnitude < 0.0001f)
            return;  // Avoid zero-length direction

        // Force Y value to 0 to prevent vertical rotation
        directionToTarget.y = 0;

        // Determine the desired rotation so that the camera looks at the target.
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Compute the angle difference between current and target rotations.
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        // Only rotate if the angle difference exceeds the deadzone.
        if (angleDifference > deadzoneAngle)
        {
            // Smoothly interpolate the rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
        }
    }
}
