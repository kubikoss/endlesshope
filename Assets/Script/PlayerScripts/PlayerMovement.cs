using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {  get; private set; }

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float sensX;
    public float sensY;
    public Rigidbody rb;
    [HideInInspector] public bool canRotateCam = true;
    float xRotation;
    float yRotation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    private AudioSource movementAudioSource;
    [SerializeField]
    AudioClip movementSound;

    [Header("Jumping")]
    public float jumpForce = 6f;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Player Drag")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movementAudioSource = gameObject.AddComponent<AudioSource>();
        movementAudioSource.loop = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);

        MovementControl();
        SpeedControl();
        PlayerMovementAnimation();
        MoveCamera();
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

    private void MoveCamera()
    {
        if(canRotateCam)
        {
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            PlayerManager.Instance.mainCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    private void PlayerMovementAnimation()
    {
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        animator.SetBool("isMoving", isMoving);

        if (!isMoving)
        {
            animator.SetBool("isIdle", true);

            if (movementAudioSource.isPlaying)
                StartCoroutine(StopWalking());
        }
        else
        {
            animator.SetBool("isIdle", false);

            if(!movementAudioSource.isPlaying)
            {
                movementAudioSource.clip = movementSound;
                movementAudioSource.volume = 1f;
                movementAudioSource.Play();
            }
        }
    }

    private IEnumerator StopWalking()
    {
        yield return new WaitForSeconds(0.2f);
        movementAudioSource.Stop();
    }
}