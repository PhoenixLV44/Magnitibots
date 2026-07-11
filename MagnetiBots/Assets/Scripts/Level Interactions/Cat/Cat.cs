using System;
using System.Collections;
using UnityEngine;

namespace Cat
{
    public class Cat : MonoBehaviour
    {
        Animator _animator;
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

        private void Start()
        {
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

        private void FixedUpdate()
        {
            /*Vector3 direction = _player.transform.position - transform.position;
            transform.localEulerAngles.y = direction.y;*/
            head.rotation = Quaternion.LookRotation(head.position - _player.transform.position);
            head.localEulerAngles = new Vector3(head.localEulerAngles.x, head.localEulerAngles.y, 0);
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

        public IEnumerator Disappear()
        {
            for (int i = 1; i < triggerSphereColliders.Length; i++)
            {
                triggerSphereColliders[i].enabled = false;
            }
            if (_smokeParticles)
            {
                _smokeParticles.Play(true);
            }

            Globals.Managers.Audio.PlaySFXHere("Meow4", transform);
            foreach (var model in renderers)
            {
                model.enabled = false;
            }

            yield return new WaitUntil(() => !_smokeParticles.GetComponent<ParticleSystem>().isPlaying);
            _catManager.ChangeCat(this);
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
    }

}