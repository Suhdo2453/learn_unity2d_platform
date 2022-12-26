using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 600f;
    public float timeSiceAttack1 = 0.4f;
    public float timeSiceAttack2 = 0.5f;
    public float timeSiceAttack3 = 0.5f;
    public float wallSlidingSpeed = 2f;

    [SerializeField] internal Rigidbody2D rb;

    [SerializeField] internal bool isGrounded;
    [SerializeField] internal bool isJumping;
    [SerializeField] internal bool isSliding;
    [SerializeField] internal bool isFalling;

    [SerializeField] internal float yVelocity;
    [SerializeField] internal int currentAttack;

    [SerializeField] internal Sensor l2_sensor;
    [SerializeField] internal Sensor r1_sensor;
    [SerializeField] internal Sensor r2_sensor;

    [SerializeField] internal InputManager inputManager;
    [SerializeField] internal PlayerAttacking playerAttacking;
    [SerializeField] internal PlayerAnimation playerAnimation;
    [SerializeField] internal PlayerMovement playerMovement;

    private void Start()
    {
        l2_sensor = transform.Find("L2_Sensor").GetComponent<Sensor>();
        r1_sensor = transform.Find("R1_Sensor").GetComponent<Sensor>();
        r2_sensor = transform.Find("R2_Sensor").GetComponent<Sensor>();

        playerMovement = transform.Find("PlayerMovement").GetComponent<PlayerMovement>();
        playerAnimation = transform.Find("Model").GetComponent<PlayerAnimation>();
        playerAttacking = transform.Find("PlayerAttacking").GetComponent<PlayerAttacking>();

        rb = transform.GetComponent<Rigidbody2D>();
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
        if (!isGrounded && l2_sensor.State() && r2_sensor.State())
        {
            isGrounded = true;
            isJumping = false;
        }

        if (isGrounded && (!l2_sensor.State() || !r2_sensor.State()))
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
