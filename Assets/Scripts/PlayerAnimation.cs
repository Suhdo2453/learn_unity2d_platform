using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] protected string currentAnimation;
    [SerializeField] protected Animator animator;

    const string PLAYER_RUN = "run";
    const string PLAYER_IDLE = "idle";
    const string PLAYER_JUMP = "jump";
    const string PLAYER_FALL = "fall";
    const string PLAYER_SLIDE = "slide";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        this.Run();
        this.Jump();
        this.Slide();
        this.Attack();
    }

    protected virtual void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    protected virtual void Run()
    {
        if (!PlayerMovement.Instance.IsGrounded || PlayerAttacking.Instance.IsAttacking) return;

        if (InputManager.Instance.HorizontalState != 0)
            ChangeAnimation(PLAYER_RUN);
        else ChangeAnimation(PLAYER_IDLE);
    }

    protected virtual void Jump()
    {
        if (PlayerMovement.Instance.IsGrounded) return;
        if (PlayerMovement.Instance.IsJumping) ChangeAnimation(PLAYER_JUMP);
        if (PlayerMovement.Instance.IsFalling) ChangeAnimation(PLAYER_FALL);
    }

    protected virtual void Slide()
    {
        if (!PlayerMovement.Instance.IsSliding) return;
       
        ChangeAnimation(PLAYER_SLIDE);
    }

    protected virtual void Attack()
    {
        if (!PlayerAttacking.Instance.IsAttacking) return;
        ChangeAnimation("attact" + PlayerAttacking.Instance.CurrentAttack);
    }
}
