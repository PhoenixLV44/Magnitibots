
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    GameObject _rangeIndicator;
    Renderer _rangeIndicatorRenderer;
    [SerializeField] private float _currentRange;
    public float CurrentRange => _currentRange;
    private float _maxRange;

    private void Start()
    {
        _rangeIndicator = transform.GetChild(2).gameObject;
        //_rangeIndicatorRenderer = _rangeIndicator.GetComponent<Renderer>();
        //_rangeIndicatorRenderer.material.color = Color.deepSkyBlue;
        _rangeIndicator.SetActive(false);
    }

    public void ChangeRangeSize(float circumference)
    {
        if (!_rangeIndicator.activeSelf)
        {
            _rangeIndicator.SetActive(true);
        } 
        //Debug.Log("Radius: " + radius);
        _rangeIndicator.transform.localScale = new Vector3(circumference, 0.1f, circumference);
        _currentRange = circumference / 2;
    }

    public void DisableRangeIndicator()
    {
        _rangeIndicator.SetActive(false);
    }
}
