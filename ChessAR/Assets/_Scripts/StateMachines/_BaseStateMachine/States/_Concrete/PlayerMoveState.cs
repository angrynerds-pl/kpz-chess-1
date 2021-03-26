using UnityEngine;


public class PlayerMoveState : BaseState
{
    public PlayerMoveState(IStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void InitializeState()
    {
        Debug.Log("PlayerMove");
    }
}
