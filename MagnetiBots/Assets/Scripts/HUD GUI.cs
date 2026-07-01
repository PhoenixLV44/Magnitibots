using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HUDGUI : MonoBehaviour
{
    private UIDocument ui;

    private Button unlockReturn;

    private VisualElement lassoPower;
    private VisualElement smashPower;
    private VisualElement jumpPower;

    private VisualElement unlockContainer;

    private VisualElement lassoUnlock;
    private VisualElement smashUnlock;
    private VisualElement jumpUnlock;

    private VisualElement blur;

    private Label merbleCount;

    private void Start()
    {
        ui = GetComponent<UIDocument>();
        unlockReturn = ui.rootVisualElement.Q("UnlocksReturnButton") as Button;
        unlockReturn.RegisterCallback<ClickEvent>(OnCLickUnlockReturn);

        blur = ui.rootVisualElement.Q("Blur");
        blur.visible = false;
        unlockContainer.visible = false;

        lassoUnlock = ui.rootVisualElement.Q("LassoUnlock");
        smashUnlock = ui.rootVisualElement.Q("SmashUnlock");
        jumpUnlock = ui.rootVisualElement.Q("JumpUnlock");

        lassoPower = ui.rootVisualElement.Q("LassoPower");
        smashPower = ui.rootVisualElement.Q("SmashPower");
        jumpPower = ui.rootVisualElement.Q("SuperJumpPower");

        merbleCount = ui.rootVisualElement.Q("MerbleCounter") as Label;
    }
    public void UnlockPopup(string ability)
    {
        PauseGame();
        unlockContainer.visible = true;
        switch (ability)
        {
            case "Lasso":
                lassoUnlock.visible = true;
                break;
            case "SuperJump":
                jumpUnlock.visible = true;
                break;
            case "Smash":
                smashUnlock.visible = true;
                break;
            default:
                UnPauseGame();
                break;
        }
    }
    private void PauseGame()
    {
        InputSystem.actions.actionMaps[0].Disable();
        InputSystem.actions.actionMaps[2].Disable();
        Debug.Log("pause");
        Globals.Managers.paused = true;
        blur.visible = true;
    }
    private void UnPauseGame()
    {
        unlockContainer.visible = false;
        blur.visible = false;
        foreach (VisualElement ve in unlockContainer.Children().ToArray())
        {
            ve.visible = false;
        }
        InputSystem.actions.actionMaps[0].Enable();
        InputSystem.actions.actionMaps[2].Enable();
        Debug.Log("pause");
        Globals.Managers.paused = false;
    }
    private void OnCLickUnlockReturn(ClickEvent click)
    {

    }
    public void UpdateGUI()
    {
        //this needs to be attached to wherever the current merble count is
        merbleCount.text = "9999";

        /* 
         * 
         * if(ability.isunlocked){
         * reveal ui for it
         * }
         *
         */
    }
}
