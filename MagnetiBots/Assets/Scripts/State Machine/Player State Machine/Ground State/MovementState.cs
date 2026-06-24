using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : GroundedState
{
    public MovementState(Player.Controller pc, StateMachine stateMachine, StateManager stateManager) : base(pc, stateMachine, stateManager) { }
    
    private Ability.StateManager _abilityManager;
    private Ability.Parent _currentAbility;

    public override void EnterState()
    {
        //Debug.Log("Entering Movement State");
        if (!_abilityManager)
        {
            _abilityManager = stateManager.gameObject.GetComponent<Ability.StateManager>();
        }
        _currentAbility = _abilityManager.StateMachine.CurrentState.Ability;
        player.Movement.moveSpeed = player.Movement.DefaultMoveSpeed;
        Cursor.lockState = CursorLockMode.None;
    }
    public override void ExitState()
    {
        //Debug.Log("Exiting Movement State");
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();
        
        if(InputSystem.actions.FindAction("Charge").IsPressed())
            stateMachine.ChangeState(stateManager.ChargeState);

        if(moveInput == Vector2.zero)
            stateMachine.ChangeState(stateManager.IdleState);
        
        if (InputSystem.actions.FindAction("Fire").IsPressed())
        {
            _currentAbility.Fire();
            stateMachine.ChangeState(stateManager.IdleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateManager.PlayerMovement.Look(stateManager.PlayerMovement.Submitted[1]);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        stateManager.PlayerMovement.Move(stateManager.PlayerMovement.Submitted[0]);
    }
}
