using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f;
    public float crouchSpeed = 2f;
    public float fallSpeed = 1f;

    [Header("Climb Settings")]
    public float climbCheckDistance = 1.5f; // How close you need to be to climb
    public LayerMask climbableLayer;        // Set this to only include climbable objects
    public float pushAmount = 0.5f;         // The amount to push the character after climbing

    [Header("Key Mapping - Static Directions")]
    public Vector3 forwardDirection = Vector3.back;
    public Vector3 backwardDirection = Vector3.forward;
    public Vector3 leftDirection = Vector3.left;
    public Vector3 rightDirection = Vector3.right;

    [Header("Ground | Falling")]
    public LayerMask groundLayer;            // What counts as ground

    private Rigidbody rb;
    private Animator anm;
    private Collider col;

    private bool isCrouching = false;
    private bool isClimbing = false;
    private bool isFalling = false;
    private bool hasClimbed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anm = GetComponent<Animator>();
        col = GetComponent<Collider>();

    }

    void FixedUpdate()
    {
        // Prevent moving if we are climbing
        if (IsClimbing())
        {
            if(col.enabled)
                col.enabled = false;
            return;
        } else
        {
            if(!col.enabled)
                col.enabled = true;
        }

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

            float currentSpeed = isCrouching ? crouchSpeed : isFalling ? fallSpeed : moveSpeed;
            rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);
        }
        else
        {
            anm.SetBool("Walking", false);
        }
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = anm.GetCurrentAnimatorStateInfo(0);
        bool currentlyClimbing = stateInfo.IsName("Climb");

        anm.applyRootMotion = currentlyClimbing;
        if (currentlyClimbing && !isClimbing)
        {
            isClimbing = true;
            hasClimbed = false;
        }

        if (isClimbing && stateInfo.normalizedTime >= 0.9f && !hasClimbed)
        {
            Vector3 pushForward = transform.forward * pushAmount;
            transform.position += pushForward;

            hasClimbed = true;
            isClimbing = false;
        }

        // Only allow checking for new climb if not climbing
        if (!currentlyClimbing)
        {
            CheckForClimb();
            bool grounded = IsGrounded();
            isFalling = !grounded;
            anm.SetBool("Falling", !grounded);
        }
    }


    bool IsClimbing()
    {
        // Prevent climb spam if we're already climbing
        if (anm.GetCurrentAnimatorStateInfo(0).IsName("Climb"))
        {
            return true;
        }

        return false;
    }
    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * 0.1f;
        float sphereRadius = 0.35f;

        return Physics.CheckSphere(origin, sphereRadius, groundLayer);
    }

    void CheckForClimb()
    {
        // Prevent climb spam if we're already climbing
        if (IsClimbing())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Vector3 direction = transform.forward;
            float radius = 0.4f;

            if (Physics.SphereCast(origin, radius, direction, out hit, climbCheckDistance, climbableLayer))
            {
                if (hit.collider.CompareTag("Climbable"))
                {
                    if (col.enabled)
                        col.enabled = false;

                    if (anm.GetBool("Falling"))
                        anm.SetBool("Falling", false);

                    anm.SetTrigger("Climbing");
                }
            }
        }
    }
}
