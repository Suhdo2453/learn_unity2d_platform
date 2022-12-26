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
    }

    protected virtual void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    protected virtual void Run()
    {
        if (!playerState.isGrounded || PlayerAttacking.Instance.IsAttacking) return;

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
        if (!PlayerAttacking.Instance.IsAttacking) return;
        ChangeAnimation("attact" + PlayerAttacking.Instance.CurrentAttack);
    }

    protected virtual void Flip()
    {
        if (PlayerAttacking.Instance.IsAttacking) return;
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
}
