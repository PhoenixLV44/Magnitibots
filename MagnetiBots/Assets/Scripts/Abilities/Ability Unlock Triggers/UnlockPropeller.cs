using System;
using UnityEngine;

namespace Abilities.Unlock
{
    public class UnlockPropeller : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Player.Controller controller = other.GetComponent<Player.Controller>();
                controller.CanUsePropeller = true;
                Player.Respawner respawner = other.GetComponent<Player.Respawner>();
                respawner.RespawnPosition = transform.position;
            }
        }
    }
}
