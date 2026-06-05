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
        player.Movement.moveSpeed = player.Movement.moveSpeed / 1.5f;
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>() ;
        
        
        if(_lassoAbility.Lever == null)
        {
            Debug.Log("No Lever");
            stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);

            _lassoAbility.MoveLassoTarget();

            player.Movement.Move(moveInput);
        }
        else
        {
            if (InputSystem.actions.FindAction("Interact").WasReleasedThisFrame())
            {
                _lassoAbility.PullLever();
            }
        }
        if (InputSystem.actions.FindAction("Charge").WasPressedThisFrame())
        {
            _lassoAbility.UnhookLasso();
        }
    }

    public override void PhysicsUpdate()
    {
        //_lassoAbility.MoveLassoTarget(moveInput);
    }
    public override void TransitionChecks()
    {
        if (!player.LassoHooked)
        {
            stateMachine.ChangeState(stateManager.IdleState);
        }
    }
}
