using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    [SerializeField] protected float horizontalState;
    [SerializeField] protected bool jumpKeyPress;
    [SerializeField] protected bool attackKeyPress;

    public static InputManager Instance { get => instance;}
    public float HorizontalState { get => horizontalState;}
    public bool JumpKeyPress { get => jumpKeyPress;}
    public bool AttackKeyPress { get => attackKeyPress;}

    private void Awake()
    {
        if (InputManager.instance != null) Debug.LogError("Only 1 InputManager allow to exsis!");
        InputManager.instance = this;
    }

    private void Update()
    {
        this.GetHorizontalState();
        this.CheckJumpKeyPress();
    }

    private void FixedUpdate()
    {
        this.CheckAttackKeyPress();
    }

    protected virtual void GetHorizontalState()
    {
        this.horizontalState = Input.GetAxisRaw("Horizontal");
    }

    protected virtual void CheckJumpKeyPress()
    {
        if (Input.GetAxisRaw("Jump") > 0) this.jumpKeyPress = true;
        else this.jumpKeyPress = false;
    }

    protected virtual void CheckAttackKeyPress()
    {
        if (Input.GetAxisRaw("Fire1") > 0) this.attackKeyPress = true;
        else this.attackKeyPress = false;
    }
}