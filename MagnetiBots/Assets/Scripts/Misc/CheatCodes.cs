using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatCodes : MonoBehaviour
{
    private void Update()
    {
        if (InputSystem.actions.FindAction("Unlock Abilities").IsPressed())
        {
            Player.Controller controller = FindFirstObjectByType<Player.Controller>();
            controller.CanUseSmash = true;
            controller.CanUsePropeller = true;
        }
    }
}
