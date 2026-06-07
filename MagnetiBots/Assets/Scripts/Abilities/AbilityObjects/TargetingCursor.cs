using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability.Object
{
    public class TargetingCursor : MonoBehaviour
    {
        private GameObject _targetCursor;
        private RangeIndicator _rangeIndicator;
        [SerializeField] private float cursorSpeed = 0.75f;
        [SerializeField] private float objectSpeed = 5;

        private void Start()
        {
            _targetCursor = transform.Find("Target Cursor").gameObject;
            _rangeIndicator = GetComponent<RangeIndicator>();

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
        
        private void MoveCursorInRange()
        {
            Vector3 playerPosition = transform.position;
            
            Vector3 cursorPosition = _targetCursor.transform.position;
            cursorPosition.y = playerPosition.y;
            
            float distance = Vector3.Distance(playerPosition, cursorPosition);
            
            float range = _rangeIndicator.CurrentRange;
            //Debug.Log("Distance: " + distance + " | Range: " + range);
            
            if (distance > range)
            {
                Debug.LogError("CURSOR OUT OF BOUNDS");
            }
        }

        public void MoveObjectToCursor(GameObject obj)
        {
            Vector3 targetPosition = MoveCursor();
            Vector3 currentPosition = obj.transform.position;
            targetPosition.y =  currentPosition.y;
            float distance = Vector3.Distance(targetPosition, currentPosition);
            
            obj.transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * objectSpeed * distance);
        }

    }
}
