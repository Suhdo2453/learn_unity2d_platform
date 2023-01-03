using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUltils : MonoBehaviour
{
    private static AnimationUltils instance;

    string currentAnimation;

    public static AnimationUltils Instance { get => instance;}

    private void Awake()
    {
        if (AnimationUltils.instance != null) Debug.LogError("Only 1 InputManager allow to exsis!");
        AnimationUltils.instance = this;
    }

    public virtual void ChangeAnimation(Animator animator, string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
