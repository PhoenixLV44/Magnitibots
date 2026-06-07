
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField]GameObject _rangeIndicator;
    Renderer _rangeIndicatorRenderer;
    private float _currentRange;
    public float CurrentRange => _currentRange;
    private float _maxRange;

    private void Start()
    {
        _rangeIndicator = transform.GetChild(2).gameObject;
        _rangeIndicatorRenderer = _rangeIndicator.GetComponent<Renderer>();
        _rangeIndicatorRenderer.material.color = Color.deepSkyBlue;
        _rangeIndicator.SetActive(false);
    }

    public void ChangeRangeSize(float radius)
    {
        if (!_rangeIndicator.activeSelf)
        {
            _rangeIndicator.SetActive(true);
        } 
        //Debug.Log("Radius: " + radius);
        _rangeIndicator.transform.localScale = new Vector3(radius, 0.1f, radius);
        _currentRange = radius;
    }

    public void DisableRangeIndicator()
    {
        _rangeIndicator.SetActive(false);
    }
}
