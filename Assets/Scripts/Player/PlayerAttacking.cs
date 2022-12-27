using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{

    [SerializeField] protected PlayerState playerState;
    [SerializeField] internal bool isAttacking;
    [SerializeField] internal int currentAttack;
    [SerializeField] internal GameObject attackHitBox;
    [SerializeField] internal float attackTime = 0.5f;
    protected float attackTime1;

    private void Start()
    {
        playerState = transform.GetComponent<PlayerState>();
        attackHitBox.SetActive(false);
    }

    private void FixedUpdate()
    {
        this.Attack();
    }

    protected virtual void Attack()
    {
        if (!InputManager.Instance.AttackKeyPress ||
            playerState.isSliding ||
            playerState.isJumping ||
            playerState.isFalling) return;
        if (!isAttacking)
        {
            currentAttack++;
            if (currentAttack > 3) currentAttack = 1;
            isAttacking = true;
            StartCoroutine(DoAttack());
        }
    }

    IEnumerator DoAttack()
    {
        attackHitBox.SetActive(true);
        if (currentAttack == 1) attackTime1 = attackTime - 0.2f;
        yield return new WaitForSeconds(attackTime1); //Sau khoang thoi gian trong () se thuc hien code ben duoi
        attackTime1 = attackTime;
        attackHitBox.SetActive(false);
        isAttacking = false;
    }
}
