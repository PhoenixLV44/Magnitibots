using UnityEngine;
using Player.States;
using UnityEngine.InputSystem;

public class LassoHooked : PlayerState
{
    public LassoHooked(Player.Controller pc, PlayerStateMachine stateMachine, PlayerStateManager stateManager) : base(pc, stateMachine, stateManager) { }

    protected Vector2 moveInput;
    private Ability.Lasso _lassoAbility;
    
    public override void EnterState()
    {
        base.EnterState();
        if (!_lassoAbility)
        {
            _lassoAbility = stateManager.gameObject.GetComponent<Ability.Lasso>();
        }
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>() ;
        
        stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
    }

    public override void PhysicsUpdate()
    {
        //_lassoAbility.MoveLassoTarget(moveInput);
    }
    public override void TransitionChecks()
    {
        if (InputSystem.actions.FindAction("Interact").IsPressed())
        {
            _lassoAbility.UnhookLasso();
            player.LassoHooked = false;
            stateMachine.ChangeState(stateManager.IdleState);
        }
    }
}
