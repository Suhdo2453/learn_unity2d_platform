using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private static PlayerAttacking instance;
    [SerializeField] private bool isAttacking;
    [SerializeField] private int currentAttack;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int CurrentAttack { get => currentAttack; set => currentAttack = value; }
    public static PlayerAttacking Instance { get => instance; }

    private void Awake()
    {
        if (PlayerAttacking.instance != null) Debug.LogError("Only 1 PlayerAttacking allow to exsis!");
        PlayerAttacking.instance = this;
    }

    private void FixedUpdate()
    {
        this.Attack();
    }

    protected virtual void Attack()
    {
        if (!InputManager.Instance.AttackKeyPress ||
            PlayerMovement.Instance.IsSliding ||
            PlayerMovement.Instance.IsJumping ||
            PlayerMovement.Instance.IsFalling) return;
        if (!isAttacking)
        {
            currentAttack++;
            if (currentAttack > 3) currentAttack = 1;
            isAttacking = true;
            Invoke("ResetAttack", 0.5f);
        }
    }

    protected virtual void ResetAttack()
    {
        isAttacking = false;
    }
}
