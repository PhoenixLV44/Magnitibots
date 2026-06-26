using UnityEngine;
using UnityEngine.InputSystem;

public class LassoHooked : GroundedState
{
    public LassoHooked(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager) : base(pc, stateMachine, stateManager) { }
    
    private Ability.Lasso _lassoAbility;
    
    public override void EnterState()
    {
        base.EnterState();
        if (!_lassoAbility)
        {
            _lassoAbility = stateManager.gameObject.GetComponent<Ability.Lasso>();
        }
        player.Movement.moveSpeed = player.Movement.moveSpeed / 1.5f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>() ;
        
        if(_lassoAbility.Lever == null)
        {
            //Debug.Log("No Lever");
            stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);

            _lassoAbility.MoveLassoTarget();

            player.Movement.Move(moveInput);
        }
        else
        {
        }
        if (InputSystem.actions.FindAction("Interact").WasReleasedThisFrame())
        {
            if (_lassoAbility.Lever != null)
            {
                if (_lassoAbility.Lever)
                {
                    _lassoAbility.PullLever();
                }
                else
                {
                   _lassoAbility.UnhookLasso();
                }
            }
            else
            {
            }
                _lassoAbility.UnhookLasso();
        }
        if (InputSystem.actions.FindAction("Charge").WasPressedThisFrame())
        {
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
