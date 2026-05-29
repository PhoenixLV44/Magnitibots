using Player.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmashAbility : PlayerAbility
{
    private void Start()
    {
        activateInput = InputSystem.actions.FindAction("ActivateSmash");
        chargeInput = InputSystem.actions.FindAction("Charge");
        fireInput = InputSystem.actions.FindAction("Fire");
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Charge()
    {
        base.Charge();
    }

    public override void Fire()
    {
        base.Fire();
    }
}
