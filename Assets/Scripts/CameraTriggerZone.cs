using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] // Ensure there's a collider for triggers
public class CameraTriggerZone : MonoBehaviour
{
    [Tooltip("The Transform defining the camera's desired position and rotation when triggered.")]
    public Transform cameraPositionTarget;

    [Tooltip("The tag of the GameObject that should trigger the camera change (e.g., 'Player').")]
    public string targetTag = "Player";

    private MainCamera_Follow mainCameraFollowScript;

    void Start()
    {
        // Find the main camera's follow script
        if (Camera.main != null)
        {
            mainCameraFollowScript = Camera.main.GetComponent<MainCamera_Follow>();
        }

        if (mainCameraFollowScript == null)
        {
            Debug.LogError("CameraTriggerZone: Could not find MainCamera_Follow script on the main camera.", this);
        }

        if (cameraPositionTarget == null)
        {
            Debug.LogError("CameraTriggerZone: Camera Position Target is not assigned.", this);
        }

        // Ensure the collider is set to be a trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError("CameraTriggerZone: No Collider found on this GameObject. Add a Collider component.", this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (cameraPositionTarget == null || mainCameraFollowScript == null) return;

        // Check if the object entering the trigger has the correct tag
        if (other.CompareTag(targetTag))
        {
            // Tell the camera to move to the fixed position, passing this zone instance
            mainCameraFollowScript.SetFixedPosition(this, cameraPositionTarget.position, cameraPositionTarget.rotation);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (mainCameraFollowScript == null) return;

        // Check if the object exiting the trigger has the correct tag
        if (other.CompareTag(targetTag))
        {
            // Tell the camera to resume following the target, passing this zone instance
            mainCameraFollowScript.ResumeFollow(this);
        }
    }

    // Optional: Draw gizmos in the editor to visualize the zone and target position
    void OnDrawGizmos()
    {
        // Draw the trigger zone bounds
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // Green, semi-transparent
            if (col is BoxCollider boxCollider)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            }
            else if (col is SphereCollider sphereCollider)
            {
                 Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                 Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
            }
             else // Fallback for other collider types (approximated)
            {
                 Gizmos.matrix = transform.localToWorldMatrix;
                 Gizmos.DrawCube(Vector3.zero, col.bounds.size / 2); // Approximation
            }
             Gizmos.matrix = Matrix4x4.identity; // Reset matrix
        }


        // Draw the camera target position
        if (cameraPositionTarget != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(cameraPositionTarget.position, 0.5f); // Draw a sphere at the target position
            Gizmos.DrawLine(transform.position, cameraPositionTarget.position); // Line from trigger center to target

            // Draw camera frustum approximation at the target
             Gizmos.matrix = Matrix4x4.TRS(cameraPositionTarget.position, cameraPositionTarget.rotation, Vector3.one);
             Gizmos.DrawFrustum(Vector3.zero, 60f, 0.5f, 0.1f, Camera.main != null ? Camera.main.aspect : 1.77f);
             Gizmos.matrix = Matrix4x4.identity; // Reset matrix
        }
    }
}
