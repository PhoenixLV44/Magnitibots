using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat
{
    public class Cat : MonoBehaviour
    {
        Animator _animator;
        public Animator Animator => _animator;
        ParticleSystem _smokeParticles;
        [SerializeField] private bool reactToCube;
        public bool  ReactToCube => reactToCube;
        [SerializeField] private bool reactToPlayer = true;
        public bool  ReactToPlayer => reactToPlayer;
        [SerializeField] private SkinnedMeshRenderer[] renderers;
        [SerializeField] SphereCollider[] triggerSphereColliders ;
        public SphereCollider[] TriggerSphereColliders {get => triggerSphereColliders; set => triggerSphereColliders = value;}
        private bool _inDanger;
        public bool InDanger => _inDanger;
        [Tooltip("Looking for J_head")]
        [SerializeField] private Transform head;
        Player.Controller _player;
        
        private CatManager _catManager;

        [SerializeField] private int triggersNeeded;
        public int  TriggersNeeded => triggersNeeded;
        private int _triggers;
        private int Triggers { get => triggersNeeded; set => triggersNeeded = value; }
        
        [Tooltip("For if the cat is in a pair with another cat")]
        [SerializeField] private Cat otherCat;
        public Cat OtherCat => otherCat;
        [Tooltip("Used for Cat that is in a pair and is the lower index in the CatManager catArray")]
        [SerializeField] private int indexIncrease = 0;
        
        Quaternion _defaultRotation;

        private void Start()
        {
            _defaultRotation = head.rotation;
            _animator = GetComponent<Animator>();
            _animator.Play("SitIdle");
            _smokeParticles = GetComponentInChildren<ParticleSystem>();
            if (_smokeParticles)
            {
                Debug.Log("Smoke Particles");
                //_smokeParticles.SetActive(false);

            }
            _player = FindFirstObjectByType<Player.Controller>();
            _catManager = GetComponentInParent<CatManager>();
        }

        private void OnEnable()
        {
            _smokeParticles = GetComponentInChildren<ParticleSystem>();
            _smokeParticles.Play(true);
            _animator = GetComponent<Animator>();
            _animator.Play("SitIdle");
            if (otherCat)
            {
                otherCat.gameObject.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            Look();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LassoTarget") || other.CompareTag("SmashBall"))
            {
                Debug.Log("MEow");
                //triggerSphereColliders = GetComponent<SphereCollider>();
                StartCoroutine(AvoidThreat());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LassoTarget") || other.CompareTag("SmashBall"))
            {
                _inDanger = false;
            }
        }

        public void IncreaseTriggersNeeded()
        {
            Debug.Log("IncreaseTriggersNeeded");
            triggersNeeded++;
        }
        public void IncreaseTriggers()
        {
            _triggers++;
            if (_triggers == triggersNeeded)
            {
                StartCoroutine(Disappear());
            }
        }
        public IEnumerator Disappear()
        {
            for (int i = 1; i < triggerSphereColliders.Length; i++)
            {
                triggerSphereColliders[i].enabled = false;
            }
            if (!_smokeParticles.isPlaying)
            {
                _smokeParticles.Play(true);
                Globals.Managers.Audio.PlaySFXHere("Meow4", transform);
            }

            foreach (var model in renderers)
            {
                model.enabled = false;
            }

            yield return new WaitUntil(() => !_smokeParticles.GetComponent<ParticleSystem>().isPlaying);
            if (!otherCat || !otherCat.gameObject.activeSelf)
            {
                _catManager.ChangeCat(this, indexIncrease);
            }
            gameObject.SetActive(false);
        }

        private IEnumerator AvoidThreat()
        {
            for (int i = 1; i < triggerSphereColliders.Length; i++)
            {
                triggerSphereColliders[i].enabled = false;
            }
            _inDanger = true;
            triggerSphereColliders[1].enabled = false;
            if (_smokeParticles)
            {
                _smokeParticles.Play(true);
            }

            Globals.Managers.Audio.PlaySFXHere("Meow4", transform);
            foreach (var model in renderers)
            {
                model.enabled = false;
            }

            yield return new WaitUntil(() => !_smokeParticles.GetComponent<ParticleSystem>().isPlaying && !_inDanger);
            
            if (_smokeParticles)
            {
                _smokeParticles.Play(true);
            }

            Globals.Managers.Audio.PlaySFXHere("Meow4", transform);
            foreach (var model in renderers)
            {
                model.enabled = true;
            }
            triggerSphereColliders[1].enabled = true;
            //gameObject.SetActive(false);
        }

        void Look()
        {
            if (!_player.GetComponent<Ability.Lasso>().LoopedObject || !_player.GetComponent<Ability.Smash>().SmashBall.activeSelf)
            {
                head.rotation = Quaternion.LookRotation(head.position - _player.transform.position);
                head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.eulerAngles.y, Mathf.Clamp(head.eulerAngles.y, 265f, 275f));
            }
            else
            {
                if (_player.GetComponent<Ability.Lasso>().LoopedObject)
                {
                    head.rotation = Quaternion.LookRotation(head.position - _player.GetComponent<Ability.Lasso>().LoopedObject.transform.position);
                    head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.eulerAngles.y, Mathf.Clamp(head.eulerAngles.y, 265f, 275f));
                }
                else
                {
                    head.rotation = Quaternion.LookRotation(head.position - _player.GetComponent<Ability.Smash>().SmashBall.transform.position);
                    head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.eulerAngles.y, Mathf.Clamp(head.eulerAngles.y, 265f, 275f));
                }
            }
        }   
    }
}