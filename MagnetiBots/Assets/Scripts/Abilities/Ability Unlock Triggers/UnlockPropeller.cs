using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Abilities.Unlock
{
    public class UnlockPropeller : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI unlockAbilityText;

        private void Start()
        {
            if (unlockAbilityText.gameObject.activeSelf)
            {
                unlockAbilityText.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Player.Controller controller = other.GetComponent<Player.Controller>();
                if (!controller.CanUseSmash)
                {
                    controller.CanUseSmash = true;
                }
                controller.CanUsePropeller = true;
                Player.Respawner respawner = other.GetComponent<Player.Respawner>();
                respawner.RespawnPosition = transform.position;
                StartCoroutine(DisplayUnlockText());
            }
        }

        IEnumerator DisplayUnlockText()
        {
            if (!unlockAbilityText)
            {
                Debug.LogWarning("Ability Unlock Text not found");
            }
            else
            {
                unlockAbilityText.gameObject.SetActive(true);
                unlockAbilityText.text = "Smash Unlocked";
            }
            yield return new WaitForSecondsRealtime(5f);
            unlockAbilityText.gameObject.SetActive(false);
        }
    }
}
