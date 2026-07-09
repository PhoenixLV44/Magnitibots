using System;
using UnityEngine;

public class PuzzleCube : MonoBehaviour
{
    private Rigidbody _rb;
    Vector3 _defaultScale;
    Vector3 _defaultRotation;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _defaultScale = transform.localScale;
        _defaultRotation = transform.localEulerAngles;
    }

    public void FreezeConstraints()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            ChangeGravity(false);
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void UnfreezeConstraints()
    {
        if (_rb != null)
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void ChangeGravity(bool value)
    {
        if (_rb)
        {
            _rb.useGravity = value;
            if (value)
            {
                _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }
    public void ResetTransform()
    {
        transform.localScale = _defaultScale;
        transform.localEulerAngles = _defaultRotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            FreezeConstraints();
            ChangeGravity(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject);
    }
}

