using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    public float moveSpeed = 5f;
    public float jumpForce = 600f;
    public float timeSiceAttack1 = 0.4f;
    public float timeSiceAttack2 = 0.5f;
    public float timeSiceAttack3 = 0.5f;
    public float wallSlidingSpeed = 2f;

    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isSliding;
    [SerializeField] private bool isFalling;

    [SerializeField] protected float yVelocity;
    [SerializeField] protected int currentAttack;

    [SerializeField] protected Sensor l2_sensor;
    [SerializeField] protected Sensor r1_sensor;
    [SerializeField] protected Sensor r2_sensor;

    Vector3 objectScale;

    public static PlayerMovement Instance { get => instance;}
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsSliding { get => isSliding; set => isSliding = value; }
    public bool IsFalling { get => isFalling; set => isFalling = value; }

    private void Awake()
    {
        if (PlayerMovement.instance != null) Debug.Log("Only 1 PlayerMovement allow to exsis!");
        PlayerMovement.instance = this;
    }

    private void Start()
    {
        l2_sensor = transform.parent.Find("L2_Sensor").GetComponent<Sensor>();
        r1_sensor = transform.parent.Find("R1_Sensor").GetComponent<Sensor>();
        r2_sensor = transform.parent.Find("R2_Sensor").GetComponent<Sensor>();

        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        yVelocity = rb.velocity.y;
        this.Jump();
        this.Move();
        this.Slide();
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

    protected virtual void Move()
    {
        if (InputManager.Instance.HorizontalState == 0) return;

        transform.parent.Translate(InputManager.Instance.HorizontalState * moveSpeed * Time.deltaTime, 0f, 0f);
        Flip();
    }

    protected virtual void Flip()
    {
        objectScale = transform.parent.localScale;

        if (InputManager.Instance.HorizontalState < 0 && objectScale.x > 0)
        {
            objectScale.x *= -1;
        }
        else if (InputManager.Instance.HorizontalState > 0 && objectScale.x < 0)
        {
            objectScale.x *= -1;
        }

        transform.parent.localScale = objectScale;
    }

    protected virtual void Jump()
    {
        CheckJumpOrFall();

        if (!isGrounded) return;

        if (InputManager.Instance.JumpKeyPress && yVelocity == 0)
        {
            rb.AddForce(new Vector2(0, jumpForce));
        }
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

    protected virtual void Slide()
    {
        if (CheckIsSlide())
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }
}
