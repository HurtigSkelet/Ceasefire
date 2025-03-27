using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Camera mainCamera;
    public float cameraSensitivity = 1f;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    public float airControlFactor = 0.5f;

    [Header("Crouch Settings")]
    public float crouchSpeedMultiplier = 0.5f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent unwanted rotation
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
        Cursor.visible = false; // Hide cursor
    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInput()
    {
        // Get WASD input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Looking around input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        mainCamera.transform.Rotate(-mouseY * cameraSensitivity, 0, 0); // Rotate camera on y-axis
        transform.Rotate(0, mouseX * cameraSensitivity, 0); // Rotate player on x-axis

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Placeholder for crouch input
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            UnCrouch();
        }

        // Placeholder for rocket jump or other advanced inputs
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Right mouse button
        {
            //RocketJump();
        }
    }

private void ApplyMovement()
{
    Vector3 velocity = rb.linearVelocity;

    if (isGrounded)
    {
        // Apply full movement control on the ground
        Vector3 targetVelocity = moveDirection * moveSpeed;
        velocity = Vector3.Lerp(velocity, targetVelocity, 0.8f); // Smoothly adjust velocity
    }
    else
    {
        // Air-strafing mechanics
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z); // Ignore vertical velocity
        Vector3 airControlDirection = moveDirection.normalized;

        // Calculate the player's current speed in the horizontal plane
        float horizontalSpeed = horizontalVelocity.magnitude;

        // If there's input, adjust the velocity based on the input direction and mouse movement
        if (moveDirection != Vector3.zero)
        {
            // Calculate the desired direction based on input and mouse movement
            Vector3 desiredDirection = airControlDirection;

            // Add rotational influence based on mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            desiredDirection = Quaternion.Euler(0, mouseX * cameraSensitivity, 0) * desiredDirection;

            // Apply acceleration in the desired direction
            Vector3 airAcceleration = desiredDirection * moveSpeed * airControlFactor * Time.fixedDeltaTime;

            // Add the acceleration to the horizontal velocity
            horizontalVelocity += airAcceleration;

            // Clamp horizontal velocity to max speed
            if (horizontalVelocity.magnitude > horizontalSpeed && horizontalVelocity.magnitude > moveSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * horizontalSpeed;
            }
        }

        // Combine horizontal and vertical velocity
        velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }

    rb.linearVelocity = velocity;
}

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    private void Crouch()
    {
        // Example crouch logic: reduce speed
        moveSpeed *= crouchSpeedMultiplier;
        // Add more crouch behavior here (e.g., change capsule height)
    }

        private void UnCrouch()
    {
        // Example crouch logic: reduce speed
        moveSpeed /= crouchSpeedMultiplier;
        // Add more crouch behavior here (e.g., change capsule height)
    }

    private void RocketJump()
    {
        // Example rocket jump logic: apply an upward force
        rb.AddForce(Vector3.up * jumpForce * 2, ForceMode.Impulse);
        // Add explosion force or other effects here
    }

private void CheckGrounded()
{
    // Define a small sphere at the player's feet for ground detection
    float groundCheckRadius = 0.2f; // Adjust this based on your player's size
    Vector3 groundCheckPosition = transform.position + Vector3.down * 0.85f; // Slightly below the player's center

    // Check if the sphere overlaps with any colliders in the ground layer
    isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, LayerMask.GetMask("Default"));
}
}