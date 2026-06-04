using Interactable;
using UnityEngine;

public class TestInteractable : InteractableObject
{
    public override void ActivateObject()
    {
        Debug.Log("Object Interacted with");
    }
}
