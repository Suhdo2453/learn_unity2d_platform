using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private static PlayerAttacking instance;

    [SerializeField] protected PlayerState playerState;
    [SerializeField] private bool isAttacking;
    [SerializeField] private int currentAttack;
    [SerializeField] private GameObject attackHitBox;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int CurrentAttack { get => currentAttack; set => currentAttack = value; }
    public static PlayerAttacking Instance { get => instance; }

    private void Awake()
    {
        if (PlayerAttacking.instance != null) Debug.LogError("Only 1 PlayerAttacking allow to exsis!");
        PlayerAttacking.instance = this;
    }

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
