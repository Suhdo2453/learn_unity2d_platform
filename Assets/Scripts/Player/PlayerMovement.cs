using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected PlayerState playerState;

    [Header("For Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float airMoveSpeed = 30f;

    [Header("For Jumping")]
    [SerializeField] float jumpForce = 16f;


    [Header("For WallJumping")]
    [SerializeField] float wallJumpForce = 600f;
    [SerializeField] float wallJumpDirection = -1f;
    [SerializeField] Vector2 wallJumpAngle;

    [Header("For WallSliding")]
    [SerializeField] float wallSlideSpeed = 0;
    [SerializeField] float distLimitWallJump;
    float xLimitWallJump;
    float xOldLimitWallJump;
    bool canMove;


    private void Start()
    {
        playerState = transform.GetComponent<PlayerState>();
        wallJumpAngle.Normalize();
    }

    private void FixedUpdate()
    {
        this.Jump();
        this.WallJump();
        this.Move();
        this.WallSlide();
    }

    protected virtual void Move()
    {
        if (playerState.playerAttacking.isAttacking || playerState.playerBlock.isBlock) return;
        playerState.rb.velocity = new Vector2(InputManager.Instance.HorizontalState * moveSpeed, playerState.rb.velocity.y);
        /*
        if (playerState.isGrounded)
        {
            playerState.rb.velocity = new Vector2(InputManager.Instance.HorizontalState * moveSpeed, playerState.rb.velocity.y);
        }
        else if(!playerState.isGrounded && !playerState.IsWallSliding && InputManager.Instance.HorizontalState != 0)
        {
            playerState.rb.AddForce(new Vector2(airMoveSpeed * InputManager.Instance.HorizontalState, 0));
            Debug.Log("Move");
            if(Mathf.Abs(playerState.rb.velocity.x) > moveSpeed)
            {
                Debug.Log("Move 2");
                playerState.rb.velocity = new Vector2(InputManager.Instance.HorizontalState * moveSpeed, playerState.rb.velocity.y);
            }
        }*/
    }

    void CheckCanMove()
    {
        xLimitWallJump = transform.position.x;
        distLimitWallJump = xLimitWallJump - xOldLimitWallJump;
    }


    protected virtual void Jump()
    {
        if (!playerState.isGrounded || playerState.playerAttacking.isAttacking) return;
        if (InputManager.Instance.JumpKeyPress)
        {
            playerState.rb.velocity = new Vector2(playerState.rb.velocity.x, jumpForce);
        }
    }

    protected virtual void WallJump()
    {
        if(playerState.IsWallSliding && InputManager.Instance.JumpKeyPress)
        {
            xOldLimitWallJump = transform.position.x;
            playerState.rb.AddForce(new Vector2(wallJumpForce * -transform.localScale.x * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Force);
            playerState.isWallJump = true;
        }
    }

    protected virtual void WallSlide()
    {
        if (playerState.IsWallSliding)
        {
            playerState.rb.velocity = new Vector2(playerState.rb.velocity.x,
                                                wallSlideSpeed);
        }
    }
}
