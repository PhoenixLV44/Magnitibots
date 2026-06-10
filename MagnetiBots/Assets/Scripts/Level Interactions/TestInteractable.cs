using Interactable;
using UnityEngine;

public class TestInteractable : InteractableObject
{
    public override void ActivateObject()
    {
        if (!activated)
        {
            activated = true;
            Debug.Log("Object Activated");
        }
    }

    public override void DeactivateObject()
    {
        if (canBeDeactivated && activated)
        {
            activated = false;
            Debug.Log("Object Deactivated");
        }
        else
        {
            Debug.Log("Cant be  deactivated");
        }
    }
}
