using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState: AirState
{
    public JumpState(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    
    

    public override void EnterState()
    {
        base.EnterState();
        player.Movement.StartCoroutine(player.Movement.Jump());
        //Cursor.lockState = CursorLockMode.None;

    }

    public override void ExitState()
    {

    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        if (player.Movement.Grounded)
        {
            stateMachine.ChangeState(stateManager.IdleState);
        }
        if (!player.Movement.IsRising())
        {
            //Debug.Log("Falling");
            if (!player.Movement.Grounded && !player.Movement.Hovering)
            {
                stateMachine.ChangeState(stateManager.FallState);
            }
            else if (!player.Movement.Grounded && player.Movement.Hovering)
            {
                stateMachine.ChangeState(stateManager.HoverState);
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
    }

    public override void PhysicsUpdate()
    {
        stateManager.PlayerMovement.Move(stateManager.PlayerMovement.Submitted[0]);
    }
}
