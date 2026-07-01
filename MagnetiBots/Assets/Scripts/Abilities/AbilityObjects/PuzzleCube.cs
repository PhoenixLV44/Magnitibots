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
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void UnfreezeConstraints()
    {
        if (_rb != null)
        {
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.None;
        }
    }
    public void ResetTransform()
    {
        transform.localScale = _defaultScale;
        transform.localEulerAngles = _defaultRotation;
    }
}

