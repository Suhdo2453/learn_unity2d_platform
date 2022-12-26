using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected PlayerState playerState;

    private void Start()
    {
        playerState = transform.parent.GetComponent<PlayerState>();
    }

    private void FixedUpdate()
    {
        this.Jump();
        this.Move();
        this.Slide();
        this.CheckIsGround();
    }

    protected virtual void CheckIsGround()
    {
        if (!playerState.isGrounded && playerState.l2_sensor.State() && playerState.r2_sensor.State())
        {
            playerState.isGrounded = true;
            playerState.isJumping = false;
        }

        if (playerState.isGrounded && (!playerState.l2_sensor.State() || !playerState.r2_sensor.State()))
        {
            playerState.isGrounded = false;
        }
    }

    protected virtual void Move()
    {
        if (InputManager.Instance.HorizontalState == 0 || PlayerAttacking.Instance.IsAttacking) return;

        transform.parent.Translate(InputManager.Instance.HorizontalState * playerState.moveSpeed * Time.fixedDeltaTime, 0f, 0f);
    }


    protected virtual void Jump()
    {
        if (!playerState.isGrounded) return;

        if (InputManager.Instance.JumpKeyPress && playerState.yVelocity == 0)
        {
            playerState.rb.AddForce(new Vector2(0, playerState.jumpForce));
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
