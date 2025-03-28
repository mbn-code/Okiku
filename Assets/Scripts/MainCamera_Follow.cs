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

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the direction from the camera to the target.
        Vector3 directionToTarget = target.position - transform.position;
        if (directionToTarget.sqrMagnitude < 0.0001f)
            return;  // Avoid zero-length direction

        // Uncomment the next line if you want to ignore vertical differences (only rotate on Y axis).
        // directionToTarget.y = 0;

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
