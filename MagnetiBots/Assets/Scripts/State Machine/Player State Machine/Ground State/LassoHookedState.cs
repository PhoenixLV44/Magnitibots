using UnityEngine;
using UnityEngine.InputSystem;

public class LassoHooked : GroundedState
{
    public LassoHooked(Player.Controller pc, Player.StateMachine stateMachine, Player.StateManager stateManager, Animator animator) : base(pc, stateMachine, stateManager, animator) { }
    
    private Ability.Lasso _lassoAbility;
    
    public override void EnterState()
    {
        base.EnterState();
        if (!_lassoAbility)
        {
            _lassoAbility = stateManager.gameObject.GetComponent<Ability.Lasso>();
        }
        animator.Play("Arm_Up");
        //player.Movement.moveSpeed = player.Movement.moveSpeed / 1.5f;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public override void ExitState()
    {
        base.ExitState();
        _lassoAbility.MerbleBoss.FireMerbles();
        _lassoAbility.StopCoroutine(_lassoAbility.ChargeCoroutine);
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>() ;
        
        if(_lassoAbility.Lever == null)
        {
            //Debug.Log("No Lever");
            stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);

            if (_lassoAbility.TargetCursor.CanMoveCursor && player.TargetCursor.RaycastPoint.activeSelf)
            {
                _lassoAbility.MoveLassoTarget();
            }

            //player.Movement.Move(moveInput);
        }
        else
        {
        }
        if (InputSystem.actions.FindAction("Interact").WasReleasedThisFrame())
        {
            /*
            if (_lassoAbility.Lever != null)
            {
                if (_lassoAbility.Lever)
                {
                    _lassoAbility.PullLever();
                }
                else
                {
                   _lassoAbility.StartCoroutine(_lassoAbility.UnhookLasso());
                }
            }
            else
            {
            }
            */
            player.Movement.CanLook = false;
            _lassoAbility.TargetCursor.CanMoveCursor = false;
            //_lassoAbility.TargetCursor.DeactivateCursor();
            _lassoAbility.StartCoroutine(_lassoAbility.UnhookLasso());
        }

    }

    public override void PhysicsUpdate()
    {
        //_lassoAbility.MoveLassoTarget(moveInput);
    }
    public override void TransitionChecks()
    {
        if (!player.LassoHooked || !player.TargetCursor.ObjectToMove || player.Respawning)
        {
            stateMachine.ChangeState(stateManager.IdleState);
        }

        if (!player.TargetCursor.ObjectToMove)
        {
            
        }
    }
}
