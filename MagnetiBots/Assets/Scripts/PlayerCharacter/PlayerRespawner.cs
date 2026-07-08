
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Player
{
    public class Respawner : MonoBehaviour
    {
        private InputAction _respawnInput;
        [SerializeField] Vector3 _respawnPosition;
        public Vector3 RespawnPosition { get { return _respawnPosition; } set { _respawnPosition = value; } }
        Controller _playerController;
        Movement _movement;
        public Movement Movement { get => _movement;
            set => _movement = value;
        }
        Merbles.Boss _boss;
        
        [SerializeField] LayerMask respawnMask;
        
        List<Interactable.ItemRespawner> _itemRespawners = new List<Interactable.ItemRespawner>();
        public List<Interactable.ItemRespawner> ItemRespawners{get => _itemRespawners; set => _itemRespawners = value; }
        [SerializeField] private SkinnedMeshRenderer[] playerModel;
        private void Start()
        {
            _respawnInput = InputSystem.actions.FindAction("Respawn");
            _respawnPosition = transform.position;
            _playerController = GetComponent<Controller>();
            if(!_movement)
                _movement = GetComponent<Movement>();
            
        }

        private void Update()
        {
            if (!_boss)
            {
                _boss = GetComponent<Merbles.Boss>();
            }
            if (_respawnInput.WasPressedThisFrame())
            {
                StartCoroutine(Respawn());
            }
        }

        private void FixedUpdate()
        {
            CheckForRespawn();
        }

        public IEnumerator Respawn()
        {
            _playerController.Respawning = true;
            _playerController.LassoHooked = false;
            Globals.Managers.Settings.FadeAway();
            _playerController.AbilityStateManager.CurrentAbility.Respawn();
            foreach (var renderer in playerModel)
            {
                renderer.enabled = false;
            }
            yield return new WaitForSecondsRealtime(1);
            foreach (var renderer in playerModel)
            {
                renderer.enabled = true;
            }
            Debug.Log("Respawn");
            _movement.CharacterController.enabled = false;
            _playerController.transform.position = _respawnPosition;
            
            /*_playerController.AbilityStateManager.CurrentAbility.StopCharging();
            _playerController.AbilityStateManager.CurrentAbility.StopAllCoroutines();
            _playerController.MerbleBoss.FireMerbles();*/
            _playerController.PlayerStateManager.StateMachine.ChangeState(_playerController.PlayerStateManager.IdleState);
            
            foreach (var merble in _boss.MasterList)
            {
                Vector2 rng = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                Vector3 newMerblePos = new Vector3(_respawnPosition.x + rng.x, _respawnPosition.y - 1, _respawnPosition.z + rng.y);
                merble.gameObject.SetActive(false);
                
                merble.transform.position = newMerblePos;
                merble.gameObject.SetActive(true);
            }

            foreach (var item in _itemRespawners)
            {
                if (item.CanRespawn)
                {
                    item.Respawn();
                }
            }
            _movement.CharacterController.enabled = true;
            _playerController.Respawning = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "RespawnPlane")
            {
                //Respawn();
            }
        }

        void CheckForRespawn()
        {
            RaycastHit hit;
            if (Physics.SphereCast((transform.position), 0.5f, -Vector3.up, out hit, 1.5f, respawnMask))
            {
                Debug.Log("Respawn Raycast");
                Globals.Managers.Audio.PlaySFX("waterSplash");
                StartCoroutine(Respawn());
            }
        }
    }

}