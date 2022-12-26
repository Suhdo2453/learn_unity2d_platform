using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState : MonoBehaviour
{

    [SerializeField] internal bool isGrounded;
    [SerializeField] internal bool isJumping;
    [SerializeField] internal bool isSliding;
    [SerializeField] internal bool isFalling;
    [SerializeField] internal bool canWallJump;
}
