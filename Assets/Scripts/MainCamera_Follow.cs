using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Follow : MonoBehaviour
{
    private enum CameraMode { FollowTarget, FixedPosition }
    private CameraMode currentMode = CameraMode.FollowTarget;

    [Header("Target Settings")]
    public Transform target;  // The GameObject the camera should follow

    [Header("Follow Mode Settings")]
    [Tooltip("Time (in seconds) for the camera to smooth its rotation when following.")]
    public float followRotationSmoothTime = 0.2f;
    [Tooltip("Minimum angle difference before the camera starts rotating when following.")]
    public float deadzoneAngle = 5f;
    [Tooltip("Time (in seconds) for the camera to smooth its position movement when following.")]
    public float followPositionSmoothTime = 0.5f;
    [Tooltip("How far the camera should maintain from the target on Z axis when following.")]
    public float offsetZ = 0f;

    [Header("Fixed Position Mode Settings")]
    [Tooltip("Time (in seconds) for the camera to smooth its position movement to the fixed target.")]
    public float fixedPositionSmoothTime = 1.0f;
    [Tooltip("Time (in seconds) for the camera to smooth its rotation to the fixed target.")]
    public float fixedRotationSmoothTime = 1.0f;

    private Vector3 currentVelocity;
    private float initialX;  // Store initial X position for follow mode
    private float initialY;  // Store initial Y position for follow mode
    private Vector3 targetFixedPosition;
    private Quaternion targetFixedRotation;

    void Start()
    {
        // Store the initial X and Y positions for follow mode
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    void LateUpdate()
    {
        switch (currentMode)
        {
            case CameraMode.FollowTarget:
                HandleFollowTarget();
                break;
            case CameraMode.FixedPosition:
                HandleFixedPosition();
                break;
        }
    }

    void HandleFollowTarget()
    {
        if (target == null)
            return;

        // Handle position following (Z axis only, maintaining initial X/Y)
        Vector3 targetPosition = transform.position;
        targetPosition.x = initialX;  // Lock X position
        targetPosition.y = initialY;  // Lock Y position
        targetPosition.z = target.position.z + offsetZ;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            followPositionSmoothTime // Use follow-specific smoothing
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
            float step = (1.0f / followRotationSmoothTime) * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }

    void HandleFixedPosition()
    {
        // Smoothly move towards the target fixed position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetFixedPosition,
            ref currentVelocity,
            fixedPositionSmoothTime // Use fixed position smoothing
        );

        // Smoothly rotate towards the target fixed rotation
        float step = (1.0f / fixedRotationSmoothTime) * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetFixedRotation,
            step
        );
    }

    // Public method to switch to fixed position mode
    public void SetFixedPosition(Vector3 position, Quaternion rotation)
    {
        targetFixedPosition = position;
        targetFixedRotation = rotation;
        currentMode = CameraMode.FixedPosition;
        currentVelocity = Vector3.zero; // Reset velocity for the new SmoothDamp operation
    }

    // Public method to switch back to following the target
    public void ResumeFollow()
    {
        currentMode = CameraMode.FollowTarget;
        currentVelocity = Vector3.zero; // Reset velocity for the new SmoothDamp operation

        // Re-capture initial X/Y based on current position to avoid snapping
        initialX = transform.position.x;
        initialY = transform.position.y;
    }
}
