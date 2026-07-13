using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private bool _disappeared;

        [SerializeField] private int triggersNeeded;
        public int  TriggersNeeded => triggersNeeded;
        private int _triggers;
        private int Triggers { get => triggersNeeded; set => triggersNeeded = value; }
        
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

            if (other.CompareTag("Player"))
            {
                int rng = Random.Range(1, 4);
                Globals.Managers.Audio.PlaySFXHere($"Meow{rng}", transform);

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
            if (!_disappeared)
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
            }
            _catManager.ChangeCat(this);
            gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator AvoidThreat()
        {
            _disappeared = true;
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
            _disappeared = true;
            //gameObject.SetActive(false);
        }

        void Look()
        {
            head.rotation = Quaternion.LookRotation(head.position - _player.transform.position);
            head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.eulerAngles.y, Mathf.Clamp(head.eulerAngles.y, 265f, 275f));
        }   
    }
}