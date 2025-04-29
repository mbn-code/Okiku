using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamMover : MonoBehaviour
{
    public Vector3 movementDirection;
    public float speed = 5f;
    public float stopDistance = 66.5f;

    void Update()
    {
        if (transform.position.z < stopDistance) // Only move if the object is within the stop distance
        {
            transform.position += movementDirection * speed * Time.deltaTime; // Move the object
        }
    }
}
