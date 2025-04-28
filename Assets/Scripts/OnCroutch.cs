using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CapsuleCollider playerCollider;
    private Rigidbody rb;

    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    private Vector3 standingCenter;
    private Vector3 crouchingCenter;

    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        if (playerCollider != null)
        {
            standingHeight = playerCollider.height;
            standingCenter = playerCollider.center;
            crouchingCenter = new Vector3(playerCollider.center.x, playerCollider.center.y / 2f, playerCollider.center.z);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopCrouch();
        }
    }

    void StartCrouch()
    {
        isCrouching = true;
        if (playerCollider != null)
        {
            playerCollider.height = crouchHeight;
            playerCollider.center = crouchingCenter;
        }
    }

    void StopCrouch()
    {
        isCrouching = false;
        if (playerCollider != null)
        {
            playerCollider.height = standingHeight;
            playerCollider.center = standingCenter;
        }
    }
}
