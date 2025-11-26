using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;

    [Header("Checks")]
    public Transform groundCheck;

    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public float moveInput;
    [HideInInspector] public bool isGrounded;
    
    private float coyoteTimer;
    private float jumpBufferTimer;

    private bool jumpRequested;

    [HideInInspector] public bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDir;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public Vector2 attackDir;
    
    private float attackBufferTimer;
    private float attackCooldownTimer;

    private float targetSpeed;

    [HideInInspector] public int facingDir = 1; // 1 = right, -1 = left
    
    private bool isFacingRight = true;

    [HideInInspector] public bool landedThisFrame;
    [HideInInspector] public bool leftGroundThisFrame;

    [HideInInspector] public bool walkStartThisFrame;
    [HideInInspector] public bool walkStopThisFrame;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GroundCheck();
        GatherInput();
        FlipSprite();
    }

    private void GroundCheck()
    {
        // --- Ground Check ---
        bool isGroundedThisFrame = Physics2D.OverlapCircle(groundCheck.position, stats.groundCheckRadius, stats.groundLayer);

        landedThisFrame = isGroundedThisFrame && !isGrounded;
        leftGroundThisFrame = !isGroundedThisFrame && isGrounded;
        
        isGrounded = isGroundedThisFrame;
        
        if (isGrounded)
            coyoteTimer = stats.coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
    }

    private void GatherInput()
    {
        // --- Horizontal Input ---
        moveInput = Input.GetAxis("Horizontal");
        walkStartThisFrame = moveInput != 0 && Mathf.Abs(rb.linearVelocity.x) < 0.1f;
        walkStopThisFrame = moveInput == 0 && Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        // Flip facing direction
        if (moveInput != 0)
            facingDir = (int)Mathf.Sign(moveInput);
        
        // --- Jump Input Buffer ---
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = stats.jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            jumpRequested = true;
            jumpBufferTimer = 0;
        }
        
        // --- Dash Input ---
        if (Input.GetButtonDown("Dash") && dashCooldownTimer <= 0 && !isDashing)
            StartDash(moveInput);

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                isDashing = false;
        }
        dashCooldownTimer -= Time.deltaTime;
        
        
        
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
    }

    void FlipSprite()
    {
        if (rb.linearVelocity.x < -0.1f && isFacingRight)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFacingRight = false;
        }

        if (rb.linearVelocity.x > 0.1f && !isFacingRight)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFacingRight = true;
        }
    }
    
    void FixedUpdate()
    {
        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
        }

        if (isDashing)
        {
            rb.linearVelocity = dashDir * stats.dashForce;
            return;
        }

        // movement
        var speed = isGrounded ? stats.groundSpeed : stats.airSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        UpdateGravity();
    }

    void UpdateGravity()
    {
        if (isDashing)
        {
            rb.gravityScale = 0;
        }
        else if (rb.linearVelocity.y < 0.1)
        {
            rb.gravityScale = stats.gravityScale * stats.fallMultiplier;
        }
        else
        {
            rb.gravityScale = stats.gravityScale;
        }
    }
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
        coyoteTimer = 0;
    }

    void StartDash(float moveInput)
    {
        Debug.Log("Dash");
        isDashing = true;
        dashTimer = stats.dashDuration;
        dashCooldownTimer = stats.dashCooldown;

        if (Mathf.Abs(moveInput) < 0.01f)
            dashDir = facingDir == 1 ? Vector2.right : Vector2.left;
        else
            dashDir = moveInput > 0 ? Vector2.right : Vector2.left;

        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            if (isGrounded) Gizmos.color = stats.groundedGizmosColor;
            else Gizmos.color = stats.onAirGizmosColor;
            Gizmos.DrawWireSphere(groundCheck.position, stats.groundCheckRadius);
        }
    }
}
