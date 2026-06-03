
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField]GameObject _rangeIndicator;
    Renderer _rangeIndicatorRenderer;

    private void Start()
    {
        _rangeIndicator = transform.GetChild(2).gameObject;
        _rangeIndicatorRenderer = _rangeIndicator.GetComponent<Renderer>();
        _rangeIndicatorRenderer.material.color = Color.deepSkyBlue;
        _rangeIndicator.SetActive(false);
    }

    public void ChangeRangeSize(float radius)
    {
        /*
        if (_rangeIndicator == null)
        {
            _rangeIndicator = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule), new Vector3(transform.position.x, 0, transform.position.z), transform.rotation, transform);
            
            _rangeIndicator.transform.localScale = new Vector3(1, 0.1f, 1);
            
            
            _rangeIndicator.GetComponent<CapsuleCollider>().enabled = false;
            
            _rangeIndicator.name = "RangeIndicator";
            
        }
        */
        if (!_rangeIndicator.activeSelf)
        {
            _rangeIndicator.SetActive(true);
        } 
        Debug.Log("Radius: " + radius);
        _rangeIndicator.transform.localScale = new Vector3(radius, 0.1f, radius);
    }

    public void DisableRangeIndicator()
    {
        _rangeIndicator.SetActive(false);
    }
}
