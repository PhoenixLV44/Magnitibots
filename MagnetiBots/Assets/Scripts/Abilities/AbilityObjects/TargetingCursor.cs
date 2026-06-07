using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability.Object
{
    public class TargetingCursor : MonoBehaviour
    {
        private GameObject _targetCursor;
        private Vector3 _playerPosition;
        private RangeIndicator _rangeIndicator;
        [SerializeField] private float cursorSpeed = 0.75f;
        [SerializeField] private float objectSpeed = 5;

        private void Awake()
        {
            _targetCursor = transform.GetChild(1).gameObject;
            _rangeIndicator = transform.Find("RangeIndicator").GetComponent<RangeIndicator>();
            /*Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule),transform.position,transform.rotation );
        
        _targetCursor.transform.localScale = new Vector3(1.25f, 0.5f, 1.25f);
        
        _targetCursor.transform.parent = transform;
        
        _targetCursor.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, _targetCursor.transform.position.z);
        
        _targetCursor.GetComponent<CapsuleCollider>().enabled = false;
        
        _targetCursor.name = "Target Cursor";
        */

            _targetCursor.SetActive(false);
        }

        public void ActivateCursor(Vector3 position)
        {
            _targetCursor.transform.position = new Vector3(position.x, 0, position.z);
            Cursor.lockState = CursorLockMode.Locked;
            _targetCursor.SetActive(true);
        }

        public void DeactivateCursor()
        {
            _targetCursor.transform.position = transform.position;
            Cursor.lockState = CursorLockMode.None;
            _targetCursor.SetActive(false);
        }

        public Vector3 GetCursorDelta()
        {
            Vector3 cursorMovement = InputSystem.actions.FindAction("Cursor Movement").ReadValue<Vector2>();

            cursorMovement.z = cursorMovement.y;
            cursorMovement.y = 0;

            return cursorMovement;
        }

        public Vector3 MoveCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Vector3 cursorMovement = GetCursorDelta();

            _targetCursor.transform.position += cursorMovement * (Time.deltaTime * cursorSpeed);
            
            MoveCursorInRange();
            
            return _targetCursor.transform.position;
        }

        public void MoveObjectToCursor(GameObject obj)
        {
            Vector3 targetPosition = MoveCursor();
            Vector3 currentPosition = obj.transform.position;
            targetPosition.y =  currentPosition.y;
            float distance = Vector3.Distance(targetPosition, currentPosition);
            
            obj.transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * objectSpeed * distance);
        }

        private void MoveCursorInRange()
        {
            float distance = Vector3.Distance(_playerPosition - transform.position, _targetCursor.transform.position);
            float range = _rangeIndicator.CurrentRange;
            if (distance > range)
            {
                
            }
        }
    }
}
