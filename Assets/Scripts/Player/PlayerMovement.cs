using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected PlayerState playerState;
    [SerializeField] protected float xWallJump = 6.5f;
    [SerializeField] protected float yWallJump = 100f;
    private float xStartWallJump = 0f;
    private float xDistWallJump = 0f;
    private bool limitVelOnWallJump;

    private void Start()
    {
        playerState = transform.GetComponent<PlayerState>();
    }

    private void FixedUpdate()
    {
        this.Jump();
        this.WallJump();
        this.LimitWallJump();
        this.Move();
        this.Slide();
    }

    protected virtual void Move()
    {
        if (InputManager.Instance.HorizontalState == 0 ||
            playerState.playerAttacking.isAttacking ||
            playerState.canWallJump || playerState.playerBlock.isBlock) return;
        transform.Translate(InputManager.Instance.HorizontalState * playerState.moveSpeed * Time.fixedDeltaTime, 0f, 0f);
    }


    protected virtual void Jump()
    {
        if (!playerState.isGrounded || playerState.playerAttacking.isAttacking) return;
        if (InputManager.Instance.JumpKeyPress && playerState.yVelocity == 0)
        {
            playerState.rb.AddForce(new Vector2(0, playerState.jumpForce));
        }
    }

    protected virtual void WallJump()
    {
        if (!playerState.canWallJump) return;
        if (InputManager.Instance.JumpKeyPress)
        {
            //transform.Translate(xWallJump * -InputManager.Instance.HorizontalState * Time.fixedDeltaTime, 0f, 0f);
            //playerState.rb.AddForce(new Vector2(0, yWallJump));
            playerState.rb.velocity = new Vector2(0f, 0f);
            playerState.rb.AddForce(new Vector2(-transform.localScale.x * xWallJump, yWallJump));
            xStartWallJump = playerState.rb.position.x;
            limitVelOnWallJump = true;
        }
    }

    protected virtual void LimitWallJump()
    {
        if (!limitVelOnWallJump) return;

        xDistWallJump = (xStartWallJump - transform.position.x) * transform.localScale.x;

        if (xDistWallJump < -3f)
        {
            limitVelOnWallJump = false;
            playerState.rb.velocity = new Vector2(0, playerState.rb.velocity.y);
        }
        else if (xDistWallJump > 1.5f)
        {
            limitVelOnWallJump = false;
            playerState.rb.velocity = new Vector2(0, playerState.rb.velocity.y);
        }
    }

    protected virtual void Slide()
    {
        if (playerState.isSliding)
        {
            playerState.rb.velocity = new Vector2(playerState.rb.velocity.x,
                                                Mathf.Clamp(playerState.rb.velocity.y,
                                                -playerState.wallSlidingSpeed,
                                                float.MaxValue));
        }
    }
}
