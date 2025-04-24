using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;                  // How fast the character moves.
    public float rotationSpeed = 360f;            // Rotation speed in degrees per second.
    public float crouchSpeed = 2f;                // Movement speed while crouching.

    [Header("Key Mapping - Static Directions")]
    public Vector3 forwardDirection = Vector3.back;      // W key (e.g., south)
    public Vector3 backwardDirection = Vector3.forward;  // S key (e.g., north)
    public Vector3 leftDirection = Vector3.left;         // A key (e.g., west)
    public Vector3 rightDirection = Vector3.right;       // D key (e.g., east)

    private Rigidbody rb;
    private Animator anm;

    private bool isCrouching = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anm = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 moveDir = Vector3.zero;

        // Detect crouching
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        anm.SetBool("Crouching", isCrouching);

        // Check for key inputs
        if (Input.GetKey(KeyCode.W)) moveDir += forwardDirection;
        if (Input.GetKey(KeyCode.S)) moveDir += backwardDirection;
        if (Input.GetKey(KeyCode.A)) moveDir += leftDirection;
        if (Input.GetKey(KeyCode.D)) moveDir += rightDirection;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            anm.SetBool("Walking", true);

            moveDir = moveDir.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
            rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);
        }
        else
        {
            anm.SetBool("Walking", false);
        }
    }
}
