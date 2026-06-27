using System;
using System.Collections;
using Merbles;
using UnityEngine;

public class SuperJumpPoint : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.5f;
    private Boss _merbleBoss;
    public Boss MerbleBoss {get => _merbleBoss; set => _merbleBoss = value; }
    private IEnumerator _moveMerblesCoroutine;
    public IEnumerator MoveMerblesCoroutine => _moveMerblesCoroutine;
    private Transform[] _merblePoints;
    public Transform[] MerblePoints => _merblePoints;
    private Player.Movement _movement;
    private Player.Controller _playerController;
    public Player.Controller PlayerController
    {
        get;
        set;
    }
    private bool _isCharging;
    public bool IsCharging { get; set; }
    private CharacterController _characterController;
    private void Start()
    {
        //_moveMerblesCoroutine = MoveMerbles();
        _merblePoints = new Transform[transform.childCount];
        for (int i = 0; i < _merblePoints.Length; i++)
        {
            _merblePoints[i] = transform.GetChild(i);
        }
        _movement = GetComponentInParent<Player.Movement>();
        _characterController = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed, 0 * Time.deltaTime);
        //Debug.Log(_isCharging);
        /*if (_isCharging)
        {
            Debug.Log("JUMP JUMP JUMP");
            Merble[] merbleArray = _merbleBoss.ChargedMerbleList.ToArray();
            foreach (var merble in _merbleBoss.ChargedMerbleList)
            {
                Debug.Log("SPINNY");
                float moveSpeed = rotationSpeed;
                if (!_movement.Grounded)
                {
                    moveSpeed = _characterController.velocity.magnitude;
                }

                int i = _merbleBoss.ChargedMerbleList.IndexOf(merble);
                Debug.Log(moveSpeed);
                //merble.FloatTowardsObject(_merblePoints[i].transform.position, i, Merble.AbilityEnum.Propeller, rotationSpeed);
                merble.transform.position = _merblePoints[i].transform.position;
            }
        }*/
    }

    public IEnumerator MoveMerbles()
    {
        while (true)
        {
            Debug.Log("MERBLES GO SPIN");
            Merble[] merbleArray = _merbleBoss.ChargedMerbleList.ToArray();
            foreach (var merble in merbleArray)
            {
                Debug.Log("SPINNY");
                float moveSpeed = rotationSpeed;
                if (!_movement.Grounded)
                {
                    moveSpeed = _characterController.velocity.magnitude;
                }

                int i = _merbleBoss.ChargedMerbleList.IndexOf(merble);
                Debug.Log(moveSpeed);
                merble.FloatTowardsObject(_merblePoints[i].transform.position, i, Merble.AbilityEnum.SuperJump, rotationSpeed);
            }
            if (merbleArray.Length > 0)
            {

                /*for (int i = 0; i < merbleArray.Length; i++)
                {
                    Debug.Log("SPINNY");
                    float moveSpeed = rotationSpeed;
                    if (!_movement.Grounded)
                    {
                        moveSpeed = _characterController.velocity.magnitude;
                    }
                    Debug.Log(moveSpeed);
                    merbleArray[i].FloatTowardsObject(_merblePoints[i].transform.position, i, Merble.AbilityEnum.Propeller, rotationSpeed);
                }*/

            }
            yield return null;  
        }
    }
}
