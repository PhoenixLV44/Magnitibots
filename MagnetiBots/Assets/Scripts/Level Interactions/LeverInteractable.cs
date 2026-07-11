using System.Collections;
using System;
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
        private Player.Controller _controller;

        /*[SerializeField] private Renderer _renderer;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;*/
        private LeverCatToy _leverCatToy;
        private GameObject _mainCamera;

        [SerializeField] private bool actInSeries = false;
        private void Start()
        {
            delayBetweenObjects = Mathf.Clamp(delayBetweenObjects, 0, Mathf.Infinity);
            if (cat)
            {
                cat.IncreaseTriggersNeeded();
            }

            foreach (var obj in interactionObjects)
            {
                obj.TriggerObject = this;
            }
            _canvas = GetComponentInChildren<Canvas>();
            _canvas.worldCamera = Camera.main;
            _canvas.gameObject.SetActive(false);
            //_renderer = transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>();
            _leverCatToy = GetComponentInChildren<LeverCatToy>();
            if (!_leverCatToy)
            {
                Debug.LogWarning("LeverCatToy not found");
            }
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            //_deactivatedLever;
            if(interactionObjects.Length <= 1)
                actInSeries = false;
        }
        public override void ActivateObject()
        {
            if (_pullalble)
            {
                if (_leverCatToy)
                {
                    Debug.Log("Activating Lever");
                    _leverCatToy.ChangeColor();
                    Globals.Managers.Audio.PlaySFXHere("leverSfx", _leverCatToy.transform);
                }
                base.ActivateObject();
                if (_playerInRange)
                {
                    _controller.Interacting = true;
                    _controller.Movement.ChangeModelRotation(transform.position);
                    _controller.Animator.SetBool("PullingLever", true);
                    StartCoroutine(_controller.AnimController.PullingLeverAnim());
                }

                if (!canBeDeactivated)
                {
                    _pullalble = false;
                }
            }
            Debug.Log("Pull lever");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered");
                if (!_controller)
                {
                    _controller = other.gameObject.GetComponent<Player.Controller>();
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
                if (_mainCamera.activeSelf)
                {
                    _canvas.transform.rotation = Quaternion.LookRotation( _canvas.transform.position - _mainCamera.transform.position);
                }
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
        protected override IEnumerator TriggerAction(bool activation)
        {
            while (true)
            {
                if (!actInSeries)
                {
                    foreach (var obj in interactionObjects)
                    {
                        if (activation)
                            obj.ActivateObject();

                        else
                            obj.DeactivateObject();

                        if (delayBetweenObjects != 0)
                            yield return new WaitForSeconds(delayBetweenObjects);
                    }
                }
                else
                {
                    bool alreadyActivated = false;
                    foreach (var obj in interactionObjects)
                    {
                        if (!alreadyActivated && !obj.Activated)
                        {
                            obj.ActivateObject();
                            obj.Activated = true;
                            yield break;
                        }
                    }
                }
                yield break;
            }
        }
    }
}
