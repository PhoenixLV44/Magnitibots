using System;
using System.Collections;
using Merbles;
using UnityEngine;

public class SuperJumpPoint : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.5f;
    private Merbles.Boss _merbleBoss;
    public Merbles.Boss MerbleBoss => _merbleBoss;
    private IEnumerator _moveMerblesCoroutine;
    public IEnumerator MoveMerblesCoroutine => _moveMerblesCoroutine;
    private Transform[] _merblePoints;
    private void Start()
    {
        StartCoroutine(GetBoss());
        _moveMerblesCoroutine = MoveMerbles();
        _merblePoints = new Transform[transform.childCount];
        for (int i = 0; i < _merblePoints.Length; i++)
        {
            _merblePoints[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed, 0 * Time.deltaTime);
    }

    IEnumerator GetBoss()
    {
        while (!_merbleBoss)
        {
            _merbleBoss = GetComponentInParent<Merbles.Boss>();
            yield return null;
        }
    }

    public IEnumerator MoveMerbles()
    {
        while (true)
        {
            Merbles.Merble[] merbleArray = _merbleBoss.ChargedMerbleList.ToArray();
            for (int i = 0; i < merbleArray.Length; i++)
            {
                merbleArray[i].FloatTowardsObject(_merblePoints[i].transform.position, i, Merble.AbilityEnum.Propeller, rotationSpeed);
            }
            yield return null;
        }
    }
}
