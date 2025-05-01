using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_FollowRotate : MonoBehaviour
{
    public Transform MainCharacter;
    public float deadZoneAngle = 10f;      // degrees
    public float rotationSpeed = 2f;       // speed of rotation

    void FixedUpdate()
    {
        if (MainCharacter == null)
            return;

        Vector3 directionToPlayer = MainCharacter.position - transform.position;
        directionToPlayer.y = 0; // Optional: ignore vertical difference
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized);

        float angleToPlayer = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleToPlayer > deadZoneAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
