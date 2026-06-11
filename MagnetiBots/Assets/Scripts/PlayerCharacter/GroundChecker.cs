using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Player.Movement movement;
    private void OnTriggerEnter(Collider other)

    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            movement.Grounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            movement.Grounded = false;
        }
    }
}
