using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    [SerializeField] protected float horizontalState;
    [SerializeField] protected bool jumpKeyPress;
    [SerializeField] protected bool attackKeyPress;
    [SerializeField] private bool blockKeyPress;
    [SerializeField] private bool dashKeyPress;

    public static InputManager Instance { get => instance;}
    public float HorizontalState { get => horizontalState;}
    public bool JumpKeyPress { get => jumpKeyPress;}
    public bool AttackKeyPress { get => attackKeyPress;}
    public bool BlockKeyPress { get => blockKeyPress;}

    private void Awake()
    {
        if (InputManager.instance != null) Debug.LogError("Only 1 InputManager allow to exsis!");
        InputManager.instance = this;
    }

    private void Update()
    {
        this.GetHorizontalState();
        this.CheckJumpKeyPress();
        this.CheckDashKeyPress();
    }

    private void FixedUpdate()
    {
        this.CheckAttackKeyPress();
        this.CheckBlockKeyPress();
    }

    protected virtual void GetHorizontalState()
    {
        this.horizontalState = Input.GetAxisRaw("Horizontal");
    }

    protected virtual void CheckJumpKeyPress()
    {
        if (Input.GetKey(KeyCode.Space)) this.jumpKeyPress = true;
        else this.jumpKeyPress = false;
    }

    protected virtual void CheckAttackKeyPress()
    {
        if (Input.GetKey(KeyCode.Mouse0)) this.attackKeyPress = true;
        else this.attackKeyPress = false;
    }

    protected virtual void CheckBlockKeyPress()
    {
        if (Input.GetKey(KeyCode.Mouse1)) this.blockKeyPress = true;
        else this.blockKeyPress = false;
    }

    protected virtual void CheckDashKeyPress()
    {
        if (Input.GetKey(KeyCode.LeftShift)) this.dashKeyPress = true;
        else this.dashKeyPress = false;
    }
}
