using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability.Object
{
    public class TargetingCursor : MonoBehaviour
    {
        private GameObject _cursor;
        public GameObject Cursor => _cursor;
        private GameObject _raycastPoint;
        public GameObject RaycastPoint => _raycastPoint;
        
        private LayerMask _groundLayers;
        private RangeIndicator _rangeIndicator;
        [SerializeField] private float cursorSpeed = 0.75f;
        [SerializeField] private float objectSpeed = 5;
        public float ObjectSpeed => objectSpeed;

        private GameObject _objectToMove;
        public GameObject ObjectToMove { get => _objectToMove; set => _objectToMove = value; }

        private Parent _currentAbility;
        public Parent CurrentAbility { get => _currentAbility; set => _currentAbility = value; }
        private Transform _returnPoint;

        private bool _atPlayer;
        public bool  AtPlayer { get => _atPlayer; }

        private void Start()
        {
            _cursor = transform.Find("Target Cursor").gameObject;
            _raycastPoint = transform.Find("CursorRaycast").gameObject;
            _rangeIndicator = GetComponent<RangeIndicator>();
            _groundLayers = GetComponent<Player.Controller>().groundLayers;

            //_cursor.SetActive(false);
            
            _returnPoint = transform.Find("ReturnPoint");
        }

        private void Update()
        {
            if (!_objectToMove.activeSelf)
            {
                _objectToMove = null;
            }
            if (!_objectToMove)
            {
                MoveCursor();
            }
            else
            {
                MoveObjectToCursor();
            }
        }

        public void ChangeCursorPosition(Vector3 position)
        {
            _cursor.transform.position = new Vector3(position.x, transform.position.y - 1, position.z);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            if (!_cursor.activeSelf)
            {
                _cursor.SetActive(true);
            }
        }

        public void DeactivateCursor()
        {
            //_targetCursor.transform.position = transform.position;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            _cursor.SetActive(false);
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
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Vector3 cursorMovement = GetCursorDelta();
            
            Quaternion cameraRotation = GetComponent<Player.Movement>().adjustedMovement;
            cursorMovement = cameraRotation * cursorMovement;
            
            _raycastPoint.transform.position += cursorMovement * (Time.deltaTime * cursorSpeed);
            
            MoveCursorInRange();
            
            return _cursor.transform.position;
        }
        
        private void MoveCursorInRange()
        {
            Vector3 playerPosition = transform.position;
            
            Vector3 cursorPosition = _cursor.transform.position;
            playerPosition.y = cursorPosition.y;
            
            float distance = Vector3.Distance(playerPosition, cursorPosition);

            float range = _rangeIndicator.gameObject.activeSelf ? _rangeIndicator.CurrentRange : 10;
            //Debug.Log("range: " + range);
            if (range == 0)
            {
                range = 15;
            }
            if (distance > range)
            {
                _cursor.transform.position =
                    Vector3.MoveTowards(cursorPosition, playerPosition, distance - range);
            }
        }

        public void MoveObjectToCursor()
        {
            Vector3 targetPosition = MoveCursor();
            //Debug.Log("Cursor Target Position: " + targetPosition);
            Vector3 currentPosition = _objectToMove.transform.position;
            targetPosition.y =  _currentAbility == GetComponent<Lasso>()? _cursor.transform.parent.position.y + 1: _cursor.transform.parent.position.y + 6;
            float distance = Vector3.Distance(targetPosition, currentPosition);
            
            _objectToMove.transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * objectSpeed * distance);
        }

        public void ReturnToPlayer()
        {
            
        }

        private Vector3 HandleRaycast()
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(_raycastPoint.transform.position, Vector3.down, 250f, _groundLayers);
            if (hits.Length > 0)
            {
                return hits[0].point;
            }
            else
            {
                Debug.LogWarning("No Raycast Hit");
                return Vector3.zero;
            }
        }
        
    }
}
