using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    [SerializeField] private bool isAttacking;
    [SerializeField] protected int currentAttack;

    protected bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void FixedUpdate()
    {
        
    }

    protected virtual void Attack()
    {
        if (!isAttacking) return;
    }
}
