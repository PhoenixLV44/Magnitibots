using UnityEngine;

namespace Interactable
{
    public class LeverCatToy : MonoBehaviour
    {
        private Renderer _renderer;
        private bool _originalMat = true;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void ChangeColor()
        {
            if (_originalMat)
            {
                Debug.Log("Changing color to red");
                _originalMat = false;
                _renderer.material = redMaterial;
            }
            else
            {
                Debug.Log("Changing color to blue");
                _originalMat = true;
                _renderer.material = blueMaterial;
            }
        }
    }
}
