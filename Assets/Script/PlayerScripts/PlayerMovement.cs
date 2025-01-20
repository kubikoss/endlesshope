using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {  get; private set; }

    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 6f;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Player Drag")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public Rigidbody rb;

    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);

        MovementControl();
        SpeedControl();
        PlayerMovementAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void MovementControl()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (grounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    private void Move()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 2f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 0.3f, ForceMode.Force);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce / 2f, ForceMode.Impulse);
    }

    private void PlayerMovementAnimation()
    {
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        animator.SetBool("isMoving", isMoving);

        if (!isMoving)
        {
            animator.SetBool("isIdle", true);
            //Debug.Log("nehybe se");
        }
        else
        {
            //Debug.Log("hybe se");
            animator.SetBool("isIdle", false);
        }
    }
}