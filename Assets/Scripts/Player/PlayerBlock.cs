using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [SerializeField] protected PlayerState playerState;
    [SerializeField] internal bool isBlock;
    [SerializeField] internal GameObject blockHitBox;

    private void Start()
    {
        playerState = transform.GetComponent<PlayerState>();
        blockHitBox.SetActive(false);
    }

    private void FixedUpdate()
    {
        this.Block();   
    }

    protected virtual void Block()
    {
        if(!playerState.isGrounded || playerState.isFalling ||
            playerState.isJumping || playerState.isSliding ||
            playerState.playerAttacking.isAttacking)
        {
            isBlock = false;
            blockHitBox.SetActive(false);
            return;
        }
        if (InputManager.Instance.BlockKeyPress)
        {
            blockHitBox.SetActive(true);
            isBlock = true;
        }
        else 
        { 
            isBlock = false;
            blockHitBox.SetActive(false);
        }
    }
}
