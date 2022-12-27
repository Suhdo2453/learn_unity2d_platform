using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] protected string currentAnimation;
    [SerializeField] protected Animator animator;
    [SerializeField] protected PlayerState playerState;
    Vector3 objectScale;

    const string PLAYER_RUN = "run";
    const string PLAYER_IDLE = "idle";
    const string PLAYER_JUMP = "jump";
    const string PLAYER_FALL = "fall";
    const string PLAYER_SLIDE = "slide";
    const string PLAYER_BLOCK_IDLE = "block_idle";

    private void Start()
    {
        this.animator = GetComponent<Animator>();
        this.playerState = transform.parent.GetComponent<PlayerState>();
    }

    private void FixedUpdate()
    {
        this.Run();
        this.Jump();
        this.Slide();
        this.Attack();
        this.Flip();
        this.Block();
    }

    protected virtual void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    protected virtual void Run()
    {
        if (!playerState.isGrounded || playerState.playerAttacking.isAttacking || playerState.playerBlock.isBlock) return;

        if (InputManager.Instance.HorizontalState != 0)
            ChangeAnimation(PLAYER_RUN);
        else ChangeAnimation(PLAYER_IDLE);
    }

    protected virtual void Jump()
    {
        if (playerState.isGrounded || playerState.isSliding) return;
        if (playerState.isJumping) ChangeAnimation(PLAYER_JUMP);
        if (playerState.isFalling) ChangeAnimation(PLAYER_FALL);
    }

    protected virtual void Slide()
    {
        if (!playerState.isSliding) return;
       
        ChangeAnimation(PLAYER_SLIDE);
    }

    protected virtual void Attack()
    {
        if (!playerState.playerAttacking.isAttacking) return;
        ChangeAnimation("attact" + playerState.playerAttacking.currentAttack);
    }

    protected virtual void Flip()
    {
        if (playerState.playerAttacking.isAttacking || playerState.playerBlock.isBlock) return;
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

    protected virtual void Block()
    {
        if (!playerState.playerBlock.isBlock) return;
        ChangeAnimation(PLAYER_BLOCK_IDLE);
    }
}
