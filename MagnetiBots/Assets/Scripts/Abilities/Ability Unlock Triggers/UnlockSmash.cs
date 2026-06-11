using System.Collections;
using TMPro;
using UnityEngine;

namespace Abilities.Unlock
{
    public class UnlockSmash : MonoBehaviour
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
                controller.CanUseSmash = true;
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
                unlockAbilityText.text = "Smash Ability Unlocked";
            }
            yield return new WaitForSecondsRealtime(5f);
            unlockAbilityText.gameObject.SetActive(false);
        }
    }
}
