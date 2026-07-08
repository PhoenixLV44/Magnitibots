using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheatCodes : MonoBehaviour
{
    private void Update()
    {
        if (InputSystem.actions.FindAction("Unlock Abilities").IsPressed())
        {
            Player.Controller controller = FindFirstObjectByType<Player.Controller>();
            controller.CanUseSmash = true;
            controller.CanUseSuperJump = true;
        }

        if (InputSystem.actions.FindAction("Load Level Two").IsPressed())
        {
            SceneManager.LoadScene(4);
        }

        if (InputSystem.actions.FindAction("LoadFinalCutscene").IsPressed())
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        }
    }
}
