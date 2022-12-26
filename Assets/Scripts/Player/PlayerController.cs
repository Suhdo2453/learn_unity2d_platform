using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float timeSiceAttack1;
    public float timeSiceAttack2;
    public float timeSiceAttack3;

    string currentAnimationState;
    Animator animator;
    Rigidbody2D rb2d;
    bool isJumpPressed;
    bool isJumping;
    bool isAttackPressed;
    bool isAttacking;
    bool isGrounded;
    float xAxis;
    float yVeloci;
    int currentAttack = 0;

    bool isWallTouch;
    bool isSliding;
    public float wallSlidingSpeed;

    Sensor l1_sensor;
    Sensor l2_sensor;
    Sensor r1_sensor;
    Sensor r2_sensor;

    //Animation States
    const string PLAYER_IDLE = "idle";
    const string PLAYER_RUN = "run";
    const string PLAYER_ATTACK_1 = "attact1";
    const string PLAYER_ATTACK_2 = "attact2";
    const string PLAYER_ATTACK_3 = "attact3";
    const string PLAYER_JUMP = "jump";
    const string PLAYER_FALL = "fall";
    const string PLAYER_WALL_TOUCH = "sliding";

    // Start is called before the first frame update
    void Start()
    {
        l1_sensor = transform.Find("L1_Sensor").GetComponent<Sensor>();
        l2_sensor = transform.Find("L2_Sensor").GetComponent<Sensor>();
        r1_sensor = transform.Find("R1_Sensor").GetComponent<Sensor>();
        r2_sensor = transform.Find("R2_Sensor").GetComponent<Sensor>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoveState();
        CheckIsGround();
        CheckWallTouch();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void ChangeAnimation(string newState)
    {
        // neu dang chay animation do roi thi k chay lai nua
        if (currentAnimationState == newState) return;

        // chay animtion
        animator.Play(newState);

        // dat lai currentState = newState
        currentAnimationState = newState;
    }

    void CheckIsGround()
    {
        if(!isGrounded && l2_sensor.State() && r2_sensor.State())
        {
            isGrounded = true;
            isJumping = false;
        }

        if(isGrounded && (!l2_sensor.State() || !r2_sensor.State()))
        {
            isGrounded = false;
        }
    }

    void CheckWallTouch()
    {
        if (yVeloci < 0 && xAxis != 0 && ((l1_sensor.State() && l2_sensor.State()) || (r1_sensor.State() && r2_sensor.State())))
        {
            isWallTouch = true;
        }
        else isWallTouch = false;
    }

    void CheckMoveState()
    {
        // kiem tra an nut sang trai hoac phai
        xAxis = Input.GetAxisRaw("Horizontal");
        yVeloci = rb2d.velocity.y;

        // kiem tra co an nut nhay k
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumpPressed = true;
        }

        // kiem tra co an nut attack k
        if (Input.GetMouseButtonDown(0))
        {
            isAttackPressed = true;
        }
    }

    void Movement()
    {
        //Move
        if (!isAttacking)
        {
            transform.Translate(xAxis * moveSpeed * Time.deltaTime, 0f, 0f);

            //Flip
            Vector3 characterScale = transform.localScale;
            if (xAxis < 0 && characterScale.x > 0)
            {
                characterScale.x *= -1;
            }
            else if(xAxis > 0 && characterScale.x < 0)
            {
                characterScale.x *= -1;
            }
            transform.localScale = characterScale;
        }
        

        //Run
        if (isGrounded && !isAttacking && !isJumping)
        {
            if (xAxis != 0)
            {
                ChangeAnimation(PLAYER_RUN);
            }
            else
            {
                ChangeAnimation(PLAYER_IDLE);
            }
        }

        //Jump
        if(isJumpPressed && isGrounded && yVeloci == 0)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
            isJumpPressed = false;
            isJumping = true;
            ChangeAnimation(PLAYER_JUMP);
        }
        else if(!l2_sensor.State() && !r2_sensor.State() && yVeloci < 0)
        {
            ChangeAnimation(PLAYER_FALL);
        }

        //Wall touch
        if (isWallTouch && !isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            ChangeAnimation(PLAYER_WALL_TOUCH);
        }
        else if (!isGrounded && ((l1_sensor.State() && l2_sensor.State()) || (r1_sensor.State() && r2_sensor.State())))
        {
            ChangeAnimation(PLAYER_FALL);
        }

        //Attack
        if (isAttackPressed)
        {
            isAttackPressed = false;
            if (isGrounded && !isAttacking)
            {
                isAttacking = true;
                currentAttack++;
                if (currentAttack > 3)
                    currentAttack = 1;

                ChangeAnimation("attact" + currentAttack);

                switch (currentAttack)
                {
                    case 1:
                        Invoke("AttackComplete", timeSiceAttack1);
                        break;
                    case 2:
                        Invoke("AttackComplete", timeSiceAttack2);
                        break;
                    case 3:
                        Invoke("AttackComplete", timeSiceAttack3);
                        break;
                }
            }
            
        }
    }

    void AttackComplete()
    {
        isAttacking = false;
    }
}
