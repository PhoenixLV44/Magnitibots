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
        [SerializeField] private SkinnedMeshRenderer[] renderers;
        [SerializeField] SphereCollider triggerSphereCollider;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _smokeParticles = GetComponentInChildren<ParticleSystem>();
            if (_smokeParticles)
            {
                Debug.Log("Smoke Particles");
                //_smokeParticles.SetActive(false);

            }

            ;
            if (!triggerSphereCollider)
            {
                triggerSphereCollider = GetComponentInChildren<SphereCollider>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        public IEnumerator Disappear()
        {
            triggerSphereCollider.enabled = false;
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
            gameObject.SetActive(false);
        }
    }

}