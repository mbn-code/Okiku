using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseSequence_Level2 : MonoBehaviour
{
    public Camera mainCamera; // Assign in inspector or automatically
    public float xOffset = -0.1f; // How far outside the left edge (negative is offscreen)
    public float yOffset = 0f;    // Vertical offset from center of camera view
    public float zOffset = 0f;    // Optional depth offset
    public float distanceFromCamera = 2f; // Distance from the camera
    public float disapearDistance = 63f; // Distance at which the object disappears

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        Vector3 viewportPosition = new Vector3(0f + xOffset, 0.5f + yOffset, mainCamera.nearClipPlane + distanceFromCamera);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);

        worldPosition.z += zOffset;

        transform.position = worldPosition;

        if(mainCamera.transform.position.z >= disapearDistance && xOffset > -2)
        {
            xOffset -= 0.01f; // Move the object further left out of view
        }
    }
}
