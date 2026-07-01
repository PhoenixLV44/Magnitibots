using System;
using System.Collections;
using UnityEngine;

public class UnlockAbilityPackage : MonoBehaviour
{
    Animator _animator;
    [SerializeField]AnimationClip clip;
    private float _clipLength;
    Player.Controller _player;
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (clip != null)
        {
            _clipLength = clip.length;
        }
}

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = other.GetComponent<Player.Controller>();
            _player.UnlockNewAbility();
            _animator.SetTrigger("Open");
            StartCoroutine(DeleteObject());
        }
    }

    IEnumerator DeleteObject()
    {
        yield return new WaitForSecondsRealtime(_clipLength);
        gameObject.SetActive(false);
    }
}
