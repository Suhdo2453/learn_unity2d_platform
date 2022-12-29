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
    [SerializeField] float wallJumpForceX = 600f;
    [SerializeField] float wallJumpForceY = 800f;
    [SerializeField] float wallJumpDirection = -1f;
    [SerializeField] Vector2 wallJumpAngle;
    [SerializeField] float wallJumpTime = 0.05f;

    [Header("For WallSliding")]
    [SerializeField] float wallSlideSpeed = 0;


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
        if (playerState.isWallJump) return;
        if (playerState.playerAttacking.isAttacking || playerState.playerBlock.isBlock)
        {
            playerState.rb.velocity = Vector2.zero;
            return;
        }
        if (playerState.isGrounded)
        {
            playerState.rb.velocity = new Vector2(InputManager.Instance.HorizontalState * moveSpeed, playerState.rb.velocity.y);
        }
        else if(!playerState.isGrounded && !playerState.IsWallSliding && InputManager.Instance.HorizontalState != 0)
        {
            playerState.rb.AddForce(new Vector2(airMoveSpeed * InputManager.Instance.HorizontalState, 0));
            if(Mathf.Abs(playerState.rb.velocity.x) > moveSpeed)
            {
                playerState.rb.velocity = new Vector2(InputManager.Instance.HorizontalState * moveSpeed, playerState.rb.velocity.y);
            }
        }
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
            playerState.rb.AddForce(new Vector2(wallJumpForceX * -transform.localScale.x * wallJumpAngle.x, wallJumpForceY * wallJumpAngle.y), ForceMode2D.Force);
            Invoke("ResetWallJump", wallJumpTime);
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

    void ResetWallJump()
    {
        playerState.isWallJump = false;
    }
}
