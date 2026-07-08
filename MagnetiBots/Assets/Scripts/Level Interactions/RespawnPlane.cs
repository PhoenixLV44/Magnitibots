using System;
using UnityEngine;

public class RespawnPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            StartCoroutine(other.GetComponent<Player.Respawner>().Respawn());
        }*/
        if (other.CompareTag("RespawnPlane"))
        {
            Debug.Log(other.transform.name+ " is Respawning");
            other.GetComponent<Interactable.ItemRespawner>().Respawn();
        }
    }
}
