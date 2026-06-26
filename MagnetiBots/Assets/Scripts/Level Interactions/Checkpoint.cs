
using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, layerMask))
        {
            Vector3 point = hit.point;
            point.y += 0.75f;
            transform.position = point;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Respawner respawner = other.GetComponent<Player.Respawner>();
            respawner.RespawnPosition = transform.position;
            respawner.Respawn();
            Globals.Managers.Saves.AddData<Vector3>("LastRespawn",transform.position);
            Destroy(gameObject);
        }
    }
}
