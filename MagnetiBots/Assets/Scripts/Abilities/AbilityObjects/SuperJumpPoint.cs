using System;
using System.Collections;
using Merbles;
using UnityEngine;

public class SuperJumpPoint : MonoBehaviour
{
    [SerializeField] private float defaultRotationSpeed = 1.5f;
    private float _rotationSpeed;
    public float RotationSpeed => _rotationSpeed;
    private Boss _merbleBoss;
    public Boss MerbleBoss {get => _merbleBoss; set => _merbleBoss = value; }
    private IEnumerator _moveMerblesCoroutine;
    public IEnumerator MoveMerblesCoroutine => _moveMerblesCoroutine;
    private Transform[] _merblePoints;
    public Transform[] MerblePoints => _merblePoints;
    private Player.Movement _movement;
    public Player.Movement Movement {get => _movement; set => _movement = value; }

    private Player.Controller _playerController;
    public Player.Controller PlayerController
    {
        get;
        set;
    }
    private bool _isCharging;
    public bool IsCharging { get; set; }
    private void Start()
    {
        //_moveMerblesCoroutine = MoveMerbles();
        _merblePoints = new Transform[transform.childCount];
        for (int i = 0; i < _merblePoints.Length; i++)
        {
            _merblePoints[i] = transform.GetChild(i);
        }
        //_movement = _playerController.Movement;
        _rotationSpeed = defaultRotationSpeed;
    }

    private void Update()
    {
        if (_movement == null)
        {
            _movement = _playerController.Movement;
        }
        else
        {
            if (_movement.Grounded)
            {
                _rotationSpeed = defaultRotationSpeed;
            }
            else if(!_movement.Grounded)
            {
                _rotationSpeed = _movement.CharacterController.velocity.magnitude;
            }
        }
        transform.Rotate(0, _rotationSpeed, 0 * Time.deltaTime);
    }
}
