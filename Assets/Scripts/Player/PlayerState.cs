using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : ObjectState
{

    public float moveSpeed = 5f;
    public float jumpForce = 600f;
    public float timeSiceAttack1 = 0.4f;
    public float timeSiceAttack2 = 0.5f;
    public float timeSiceAttack3 = 0.5f;
    public float wallSlidingSpeed = 2f;

    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] protected BoxCollider2D boxCollider2d;
    [SerializeField] protected LayerMask layerMask;


    [SerializeField] internal float yVelocity;

    [SerializeField] internal Sensor r1_sensor;

    [SerializeField] internal PlayerAttacking playerAttacking;
    [SerializeField] internal PlayerAnimation playerAnimation;
    [SerializeField] internal PlayerMovement playerMovement;

    private void Start()
    {
        r1_sensor = transform.Find("R1_Sensor").GetComponent<Sensor>();

        playerMovement = transform.Find("PlayerMovement").GetComponent<PlayerMovement>();
        playerAnimation = transform.Find("Model").GetComponent<PlayerAnimation>();
        playerAttacking = transform.Find("PlayerAttacking").GetComponent<PlayerAttacking>();

        rb = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        yVelocity = rb.velocity.y;
        this.CheckIsSlide();
        this.CheckJumpOrFall();
        this.CheckIsGround();
    }

    protected virtual void CheckIsGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, 
                                    new Vector2(boxCollider2d.bounds.size.x - 0.1f, boxCollider2d.bounds.size.y),
                                    0f, Vector2.down, 0.1f, layerMask);
        if (raycastHit.collider != null)
        {
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    protected virtual bool CheckIsSlide()
    {
        if (yVelocity >= 0 || isGrounded || !r1_sensor.State())
        {
            isSliding = false;
            return false;
        }

        if (InputManager.Instance.HorizontalState != 0)
        {
            isSliding = true;
            isFalling = false;
        }
        else isSliding = false;

        return this.isSliding;
    }

    protected virtual void CheckJumpOrFall()
    {
        if (yVelocity == 0)
        {
            isJumping = false;
            isFalling = false;
            return;
        }
        if (yVelocity > 0)
        {
            isJumping = true;
            isFalling = false;
        }
        else if(yVelocity < 0)
        {
            isJumping = false;
            isFalling = true;
        }
    }
}