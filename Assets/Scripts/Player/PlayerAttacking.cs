using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{

    [SerializeField] protected PlayerState playerState;
    [SerializeField] internal bool isAttacking;
    [SerializeField] internal int currentAttack;
    [SerializeField] internal GameObject attackHitBox;

    private void Start()
    {
        playerState = transform.parent.GetComponent<PlayerState>();
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
        yield return new WaitForSeconds(0.5f); //Sau khoang thoi gian trong () se thuc hien code ben duoi
        attackHitBox.SetActive(false);
        isAttacking = false;
    }
}
