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

    [Header("Pull Settings")]
    public float pullCheckDistance = 1.5f;    // How close to start pulling
    public LayerMask pullableLayer;           // Only Pullable objects
    public float pullSpeed = 3f;              // Speed when pulling

    [Header("Climb Settings")]
    public float climbCheckDistance = 1.5f; // How close you need to be to climb
    public LayerMask climbableLayer;        // Set this to only include climbable objects
    public float pushAmount = 1f;         // The amount to push the character after climbing

    // Add these new variables
    public float characterScale = 1f;

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
    private bool canClimb = true;

    // —— New pull state ——
    private bool isPulling = false;
    private Transform pullTarget;
    private bool canPull = true;

    private Vector3 climbTargetPosition;
    public float climbHeightOffset = 0.1f;  // Adjust this value if needed

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anm = GetComponent<Animator>();
        col = GetComponent<Collider>();

        // Store the original scale for reference
        characterScale = transform.localScale.y;
    }

    void FixedUpdate()
    {
        // �� Handle pulling first ��
        if (isPulling)
        {
            HandlePulling();
            return;
        }

        // Prevent moving if we are climbing
        if (IsClimbing())
        {
            if (col.enabled) col.enabled = false;
            return;
        }
        else if (!col.enabled)
        {
            col.enabled = true;
        }

        if(IsLanding())
        {
            return; // Can't move when landing
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
        // Update scale if it changes during runtime
        characterScale = transform.localScale.y;

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
            // Position the character on top of the object
            transform.position = new Vector3(
                climbTargetPosition.x,
                climbTargetPosition.y,
                climbTargetPosition.z
            );

            StartCoroutine(ClimbDead());

            hasClimbed = true;
            isClimbing = false;
        }

        // �� Only check for new climb or pull if not already climbing or pulling �� 
        if (!currentlyClimbing && !isPulling)
        {
            CheckForClimb();

            bool grounded = IsGrounded();
            isFalling = !grounded;
            anm.SetBool("Falling", !grounded);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isPulling)
                AttemptPull();
            else
                StopPull();
        }
    }

    // �� Pulling logic ��

    void HandlePulling()
    {
        if (pullTarget == null)
        {
            StopPull();
            return;
        }

        // 1) Always face the object
        Vector3 pivot = pullTarget.position;
        Vector3 toTarget = (pivot - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(toTarget),
            rotationSpeed * Time.fixedDeltaTime
        );

        float dt = Time.fixedDeltaTime;

        // 2) Backwards (S) = linear pull
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 move = -toTarget * pullSpeed * dt;
            rb.MovePosition(rb.position + move);

            if (pullTarget.TryGetComponent<Rigidbody>(out var trgRb))
                trgRb.MovePosition(trgRb.position + move);
        }
        else
        {
            // 3) Sideways (A / D) = rotation around pivot
            float side = 0f;
            if (Input.GetKey(KeyCode.A)) side = 1f;  // rotate left
            if (Input.GetKey(KeyCode.D)) side = -1f;  // rotate right

            if (Mathf.Abs(side) > 0.01f)
            {
                // angular speed = (linear speed / radius) in radians/sec,
                // then convert to degrees for AngleAxis
                float radius = Vector3.Distance(rb.position, pivot);
                if (radius > 0.01f)
                {
                    float angleDeg = side * (pullSpeed / radius) * Mathf.Rad2Deg * dt;

                    // rotate character around object
                    Vector3 offset = rb.position - pivot;
                    Vector3 newPos = Quaternion.AngleAxis(angleDeg, Vector3.up) * offset + pivot;
                    rb.MovePosition(newPos);

                    // re-face the object
                    transform.rotation = Quaternion.LookRotation((pivot - newPos).normalized);

                    // rotate object itself
                    if (pullTarget.TryGetComponent<Rigidbody>(out var trgRb))
                    {
                        trgRb.MoveRotation(trgRb.rotation * Quaternion.AngleAxis(angleDeg, Vector3.up));
                    }
                    else
                    {
                        pullTarget.RotateAround(pivot, Vector3.up, angleDeg);
                    }
                }
            }
        }

        // 4) Auto-stop if you stray too far
        float dist = Vector3.Distance(transform.position, pivot);
        if (dist > pullCheckDistance * characterScale * 1.2f)
        {
            Debug.Log("Too far, stopping pull");
            StopPull();
        }
    }


    void AttemptPull()
    {
        if (!canPull) return;

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * (0.5f * characterScale);
        Vector3 dir = transform.forward;
        float radius = 0.4f * characterScale;
        float dist = pullCheckDistance * characterScale;

        if (Physics.SphereCast(origin, radius, dir, out hit, dist, pullableLayer)
            && hit.collider.CompareTag("Pullable"))
        {
            // Begin pulling
            isPulling = true;
            canPull = false;
            pullTarget = hit.collider.transform;
            anm.SetBool("Pulling", true);
            Debug.Log("Started Pulling");
        }
    }

    void StopPull()
    {
        isPulling = false;
        canPull = true;
        pullTarget = null;
        anm.SetBool("Pulling", false);

        Vector3 Rot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0f, Rot.y, 0f);
    }

    // �� Climbing helpers ��

    bool IsClimbing()
    {
        return anm.GetCurrentAnimatorStateInfo(0).IsName("Climb");
    }

    bool IsLanding()
    {
        return anm.GetCurrentAnimatorStateInfo(0).IsName("Fall_Landing");
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * (0.1f * characterScale);
        float sphereRadius = 0.35f * characterScale;
        return Physics.CheckSphere(origin, sphereRadius, groundLayer);
    }

    void CheckForClimb()
    {
        if (IsClimbing() || !canClimb)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up * (0.5f * characterScale);
            Vector3 direction = transform.forward;
            float radius = 0.4f * characterScale;
            float scaledCheckDistance = climbCheckDistance * characterScale;

            if (Physics.SphereCast(origin, radius, direction, out hit, scaledCheckDistance, climbableLayer))
            {
                if (hit.collider.CompareTag("Climbable"))
                {
                    canClimb = false;
                    if (col.enabled)
                        col.enabled = false;

                    if (anm.GetBool("Falling"))
                        anm.SetBool("Falling", false);

                    // Store the target position when starting the climb
                    climbTargetPosition = new Vector3(
                        hit.point.x,
                        hit.collider.bounds.max.y + climbHeightOffset,
                        hit.point.z
                    );

                    anm.SetTrigger("Climbing");
                }
            }
        }
    }
    IEnumerator ClimbDead()
    {
        yield return new WaitForSeconds(0.25f);
        canClimb = true;
    }
}