using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;            // How fast the character moves.
    public float rotationSpeed = 360f;      // Rotation speed in degrees per second.

    [Header("Key Mapping - Static Directions")]
    // Define the static world directions for each key.
    public Vector3 forwardDirection = Vector3.back;   // W key (e.g., south)
    public Vector3 backwardDirection = Vector3.forward; // S key (e.g., north)
    public Vector3 leftDirection = Vector3.left;        // A key (e.g., west)
    public Vector3 rightDirection = Vector3.right;      // D key (e.g., east)

    private Rigidbody rb;

    private Animator anm;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anm = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 moveDir = Vector3.zero;

        // Check for key inputs. The directions are fixed (static), not relative to character orientation.
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += forwardDirection;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += backwardDirection;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += leftDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += rightDirection;
        }

        // If there's any input, move and rotate the character.
        if (moveDir.sqrMagnitude > 0.01f)
        {
            anm.SetBool("Walking", true);
            // Normalize to ensure consistent speed regardless of diagonal movement.
            moveDir = moveDir.normalized;

            // Determine the target rotation based on the desired movement direction.
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            // Smoothly rotate the character towards the target rotation.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Move the character using Rigidbody.MovePosition to keep physics interactions smooth.
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        } else
        {
            anm.SetBool("Walking", false);
        }
    }
}
