using System;
using System.Collections;
using System.Collections.Generic;
using Merbles;
using UnityEngine;

namespace Ability.Object
{
    public class SmashBall : MonoBehaviour
    {
        [SerializeField] private float _powerLevel = 0;
        public float PowerLevel { get => _powerLevel; set => _powerLevel = value; }
        private Ability.Smash _smashAbility;
        public Ability.Smash SmashAbility { get => _smashAbility; set => _smashAbility = value; }
        [SerializeField] SphereCollider triggerCollider;
        public SphereCollider TriggerCollider => triggerCollider;
        
        private Vector3 _baseScale = new Vector3(1f, 1f, 1f);
        public Vector3 BaseScale => _baseScale;

        private List<Merbles.Merble> _merbleList;
        public List<Merbles.Merble> MerbleList { get => _merbleList; set => _merbleList = value; }
        [SerializeField]private Transform[] merblePoints;
        public Transform[] MerblePoints => merblePoints;
        
        private Rigidbody _rb;
        [SerializeField] private LayerMask groundLayer;

        
        private void OnTriggerEnter(Collider other)
        {
            triggerCollider.enabled = false;
            if (other.CompareTag("SmashTarget"))
            {
                //Globals.Managers.Audio.PlaySFXHere("ThrowRock", transform);

                SmashableTarget target = other.GetComponent<SmashableTarget>();
                target.DecreaseHealth(_powerLevel);
                Debug.Log("SmashBall: Target Health: " + target.Health);
                
                if (_powerLevel >= target.Health)
                {
                    if (target.Cat)
                    {
                        Debug.Log("SmashBall: Target has a cat");
                        target.Cat.IncreaseTriggers();
                    }
                    target.Rock.SetActive(false);
                    foreach (var boxCollider in target.Colliders)
                    {
                        boxCollider.enabled = false;
                    }
                }
                else
                {  
                    Debug.Log("SmashBall: Disable Ball");
                    _smashAbility.DeactivateBall();
                }

            }
            else if (other.CompareTag("Ground") && _rb.linearVelocity.y < 0)
            {
                //Globals.Managers.Audio.PlaySFXHere("ThrowRock", transform);
                Debug.Log("Ground");
                _smashAbility.DeactivateBall();
                //_smashAbility.MerbleBoss.FireMerbles();
            }
            else if (!other.CompareTag("Ground") && !other.CompareTag("SmashTarget") && _rb.linearVelocity.y < 0)
            {   
                Debug.Log(other.transform.name);
                _smashAbility.DeactivateBall();
            }
            Globals.Managers.Audio.PlaySFXHere("ThrowRock", transform);
        }

        public void IncreasePowerLevel(float newPowerLevel)
        {
            _powerLevel = newPowerLevel;
            transform.localScale = _baseScale * (_powerLevel);
        }
        private void OnEnable()
        {
            //_powerLevel = 1;
            transform.localScale = _baseScale;
            if (!_rb)
            {
                _rb = GetComponent<Rigidbody>();
            }
            //Debug.Log("Activating Smash Ball");
            //triggerCollider.enabled = false;
        }

        private void OnDisable()
        {
            //Cursor.lockState = CursorLockMode.None;
            StopAllCoroutines();
        }

        public IEnumerator DropBall()
        {
            while (true)
            {
                _rb.AddForce(Physics.gravity * 1.5f, ForceMode.Acceleration);
                yield return null;
            }
        }

        private void Update()
        {
            _powerLevel = Mathf.FloorToInt(_smashAbility.CurrentPowerLevel / 5);
            Debug.Log("PowerLevel: " + _powerLevel);
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1, groundLayer))
            {
                if (hitInfo.collider.CompareTag("Ground"))
                {
                    //Debug.Log("Ground");
                    _smashAbility.DeactivateBall();
                    _smashAbility.ReturnMerbles = true;
                    _smashAbility.ReturnPoint = hitInfo.point;
                    //Debug.Log(hitInfo.point);
                    //_smashAbility.MerbleBoss.FireMerbles();
                }
            }
            transform.Rotate(0, 1.5f, 0 * Time.deltaTime);
        }
    }
}
