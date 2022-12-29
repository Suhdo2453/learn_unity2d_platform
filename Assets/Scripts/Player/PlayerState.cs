using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : ObjectState
{

    [SerializeField] internal bool isGrounded;
    bool isTouchingWall;
    bool isWallSliding;
    bool isDash;
    [SerializeField] internal bool isJumping;
    [SerializeField] internal bool isSliding;
    [SerializeField] internal bool isFalling;
    [SerializeField] internal bool canWallJump;
    [SerializeField] internal bool isWallJump;

    public float moveSpeed = 5f;
    public float jumpForce = 300f;
    public float timeSiceAttack1 = 0.4f;
    public float timeSiceAttack2 = 0.5f;
    public float timeSiceAttack3 = 0.5f;
    public float wallSlidingSpeed = 2f;
    public float wallJumpTime = 0.08f;

    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] protected BoxCollider2D boxCollider2d;
    [SerializeField] protected LayerMask layerMask;


    [SerializeField] internal float yVelocity;

    [SerializeField] Transform wallCheck;
    [SerializeField] Transform groundCheck;

    [SerializeField] internal PlayerAttacking playerAttacking;
    [SerializeField] internal PlayerAnimation playerAnimation;
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal PlayerBlock playerBlock;


    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Vector2 wallCheckSize;

    public bool IsWallSliding { get => isWallSliding; set => isWallSliding = value; }
    public bool IsTouchingWall { get => isTouchingWall; set => isTouchingWall = value; }
    public bool IsDash { get => isDash; set => isDash = value; }

    private void Start()
    {
        playerMovement = transform.GetComponent<PlayerMovement>();
        playerAnimation = transform.Find("Model").GetComponent<PlayerAnimation>();
        playerAttacking = transform.GetComponent<PlayerAttacking>();
        playerBlock = transform.GetComponent<PlayerBlock>();

        rb = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        this.CheckWorld();
    }

    private void FixedUpdate()
    {
        yVelocity = rb.velocity.y;
        this.CheckJumpOrFall();
        this.WallSlide();

    }

    void CheckWorld()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, layerMask);
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, layerMask);
    }


    void WallSlide()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            isWallJump = false;
        }
        else isWallSliding = false;
    }
    

    protected virtual void CheckJumpOrFall()
    {
        if(isGrounded)
        {
            isJumping = false;
            isFalling = false;
            isWallJump = false;
            return;
        }
        if (yVelocity > 0)
        {
            isJumping = true;
            isFalling = false;
        }
        else if(yVelocity < 0 && !isGrounded)
        {
            isJumping = false;
            isFalling = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
    
}
