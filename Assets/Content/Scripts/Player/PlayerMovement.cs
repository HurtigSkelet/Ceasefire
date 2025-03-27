using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed;

    public float groundDrag;

    public float jumpPower;
    public float jumpCooldown;
    bool canJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        //Check ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight*0.5f+0.2f, groundMask);

        MyInput();
        SpeedControl();

        //Ground drag
        if(grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction based on input
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Apply movement force on the ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        // Calculate the player's current horizontal velocity
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Limit only the velocity caused by player input
        if (flatVel.magnitude > speed)
        {
            // Scale the velocity down to the maximum allowed speed
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump(){
        //reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void ResetJump(){
        canJump = true;
    }
}