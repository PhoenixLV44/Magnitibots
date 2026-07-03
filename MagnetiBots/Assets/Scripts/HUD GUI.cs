using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HUDGUI : MonoBehaviour
{
    private Player.Controller controller;
    
    private UIDocument ui;
    private VisualElement HUDContainer;

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
    private VisualElement merbleUI;

    private void Start()
    {
        Startup();
    }
    private void Startup()
    {
        ui = GetComponent<UIDocument>();
        unlockContainer = ui.rootVisualElement.Q("Unlocks");
        HUDContainer = ui.rootVisualElement.Q("MainHUD");

        unlockReturn = ui.rootVisualElement.Q("UnlocksReturnButton") as Button;
        unlockReturn.RegisterCallback<ClickEvent>(OnCLickUnlockReturn);

        blur = ui.rootVisualElement.Q("Blur");
        blur.visible = false;
        unlockContainer.visible = false;

        lassoUnlock = unlockContainer.Q("LassoUnlock");
        smashUnlock = unlockContainer.Q("SmashUnlock");
        jumpUnlock = unlockContainer.Q("SuperJumpUnlock");

        lassoPower = HUDContainer.Q("LassoPower");
        smashPower = HUDContainer.Q("SmashPower");
        jumpPower = HUDContainer.Q("SuperJumpPower");

        merbleCount = HUDContainer.Q("MerbleCounter") as Label;
        merbleUI = HUDContainer.Q("Merbles");
    }
    private void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            controller = GameObject.Find("PlayerPrefab").GetComponent<Player.Controller>();
            Startup();
        }
    }
    public void UnlockPopup(string ability)
    {
        PauseGame();
        unlockContainer.visible = true;
        switch (ability)
        {
            case "Lasso":
                Debug.Log("unlock lasso");
                lassoUnlock.visible = true;
                lassoPower.visible = true;
                break;
            case "SuperJump":
                Debug.Log("unlock SuperJump");
                jumpUnlock.visible = true;
                jumpPower.visible = true;
                break;
            case "Smash":
                Debug.Log("unlock Smash");
                smashUnlock.visible = true;
                smashPower.visible = true;
                break;
            default:
                UnPauseGame();
                break;
        }
    }
    private void PauseGame()
    {
        HUDContainer.visible = false;
        InputSystem.actions.actionMaps[0].Disable();
        InputSystem.actions.actionMaps[2].Disable();
        Time.timeScale = 0.001f;
        Debug.Log("pause");
        Globals.Managers.paused = true;
        blur.visible = true;
        unlockReturn.visible = true;
    }
    private void UnPauseGame()
    {
        HUDContainer.visible = true;
        unlockContainer.visible = false;
        blur.visible = false;
        foreach (VisualElement ve in unlockContainer.Children().ToArray())
        {
            ve.visible = false;
        }
        InputSystem.actions.actionMaps[0].Enable();
        InputSystem.actions.actionMaps[2].Enable();
        Time.timeScale = 1;
        Debug.Log("pause");
        Globals.Managers.paused = false;
    }
    private void OnCLickUnlockReturn(ClickEvent click)
    {
        UnPauseGame();
    }
    public void UpdateGUI()
    {
        HUDContainer.visible = true;
        if (controller != null)
        {
            if (controller.MerbleBoss.MasterList != null)
            {
                int realcount = controller.MerbleBoss.MasterList.Count + 1;
                merbleCount.text = "x" + realcount.ToString();
            }
        }
    }
}
