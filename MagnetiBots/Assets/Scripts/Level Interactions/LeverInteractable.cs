
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactable
{
    public class Lever : TriggerObject
    {
        bool _pullalble;
        public bool Pullalble{get => _pullalble; set => _pullalble = value; }
        private bool _playerInRange;
        public bool  PlayerInRange => _playerInRange;
        private Canvas _canvas;
        private Player.Controller controller;
        private void Start()
        {
            delayBetweenObjects = Mathf.Clamp(delayBetweenObjects, 0, Mathf.Infinity);
            _canvas = GetComponentInChildren<Canvas>();
            _canvas.worldCamera = Camera.main;
            _canvas.gameObject.SetActive(false);
        }
        public override void ActivateObject()
        {
            base.ActivateObject();
            if (_playerInRange)
            {
                controller.Interacting = true;
                controller.Movement.ChangeModelRotation(transform.position);
                controller.Animator.SetBool("PullingLever", true);
                StartCoroutine(controller.AnimController.PullingLeverAnim());
            }
            Debug.Log("Pull lever");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered");
                if (!controller)
                {
                    controller = other.gameObject.GetComponent<Player.Controller>();
                }
                _pullalble = true;
                _playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _pullalble = false;
                _playerInRange = false;
            }
        }

        private void Update()
        {
            IfLeverPullable();
        }

        void IfLeverPullable()
        {
            if (_pullalble)
            {
                _canvas.gameObject.SetActive(true);
                _canvas.transform.rotation = Quaternion.LookRotation( _canvas.transform.position - Camera.main.transform.position);
                if (_playerInRange && InputSystem.actions.FindAction("Interact").WasPressedThisFrame())
                {
                    ActivateObject();
                }
            }
            else
            {
                _canvas.gameObject.SetActive(false);
            }
        }
    }
}
