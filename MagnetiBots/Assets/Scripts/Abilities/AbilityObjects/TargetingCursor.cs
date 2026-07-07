using System;
using System.Collections;
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

        private bool _canMoveCursor = true;
        public bool  CanMoveCursor {get => _canMoveCursor; set => _canMoveCursor = value; }
        
        Player.Controller _player;
        

        private void Start()
        {
            _raycastPoint = transform.GetChild(1).gameObject;
            _cursor = _raycastPoint.transform.GetChild(0).gameObject;
            _rangeIndicator = GetComponent<RangeIndicator>();
            _groundLayers = GetComponent<Player.Controller>().groundLayers;
            _player  = GetComponent<Player.Controller>();
            //_cursor.SetActive(false);
            
            _returnPoint = GetComponent<Player.Controller>().ReturnPoint;
            if (_player.Movement)
            {
               ActivateCursor();
            }
        }

        private void Update()
        {
            if (_objectToMove && !_objectToMove.activeSelf)
            {
                _objectToMove = null;
            }

            if (_canMoveCursor && !Globals.Managers.paused)
            {
                if (!_objectToMove)
                {
                    MoveCursor();
                }
                else if(_objectToMove)
                {
                    MoveObjectToCursor();
                }
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

        public void ActivateCursor()
        {
            _raycastPoint.SetActive(true);
            _raycastPoint.transform.position = new Vector3(_player.ReturnPoint.transform.position.x, _player.ReturnPoint.transform.position.y + 25, _player.ReturnPoint.transform.position.z);
            _canMoveCursor = true;
        }

        public void DeactivateCursor()
        {
            //_targetCursor.transform.position = transform.position;
            //UnityEngine.Cursor.lockState = CursorLockMode.None;
            _raycastPoint.SetActive(false);
        }

        public Vector3 GetCursorDelta()
        {
            Vector3 cursorMovement = InputSystem.actions.FindAction("Cursor Movement").ReadValue<Vector2>();

            cursorMovement.z = cursorMovement.y;
            cursorMovement.y = 0;
            
            return cursorMovement;
        }

        private Vector3 MoveCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Vector3 cursorMovement = GetCursorDelta();
            
            Quaternion cameraRotation = GetComponent<Player.Movement>().adjustedMovement;
            cursorMovement = cameraRotation * cursorMovement;
            
            _raycastPoint.transform.position += cursorMovement * (Time.deltaTime * cursorSpeed);
            
            MoveCursorInRange();
            
            _cursor.transform.position = HandleRaycast();
            
            return _cursor.transform.position;
        }
        
        private void MoveCursorInRange()
        {
            Vector3 playerPosition = transform.position;
            
            Vector3 cursorPosition = _raycastPoint.transform.position;
            playerPosition.y = cursorPosition.y;
            
            float distance = Vector3.Distance(playerPosition, cursorPosition);

            float range = _rangeIndicator.gameObject.activeSelf ? _rangeIndicator.CurrentRange : 5f;
            //Debug.Log("range: " + range);
            if (range == 0)
            {
                range = 5f;
            }
            if (distance > range)
            {
                Vector3 targetPos = transform.position;
                targetPos.y = transform.position.y + 50;
                _raycastPoint.transform.position = Vector3.MoveTowards(cursorPosition, targetPos, distance - range);
                _cursor.transform.position = HandleRaycast();
            }
        }

        private void MoveObjectToCursor()
        {
            Vector3 targetPosition = MoveCursor();
            float height = transform.position.y;
            //Debug.Log("Cursor Target Position: " + targetPosition);
            if (_cursor.transform.position.y < transform.position.y)
            {
                height =  _currentAbility == GetComponent<Lasso>()? transform.position.y + 1.5f: transform.position.y + 6;
            }
            else
            {
                height =  _currentAbility == GetComponent<Lasso>()? _cursor.transform.position.y + 1.5f: _cursor.transform.position.y + 6;
            }
            targetPosition.y = height;
            Vector3 currentPosition = _objectToMove.transform.position;
            float distance = Vector3.Distance(targetPosition, currentPosition) > 1 ? Vector3.Distance(targetPosition, currentPosition) : 1;
            
            _objectToMove.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * objectSpeed * distance);
        }
        

        private Vector3 HandleRaycast()
        {
            RaycastHit[] hits;
            Vector3 returnPos;
            hits = Physics.RaycastAll(_raycastPoint.transform.position, Vector3.down, 250f, _groundLayers);
            Vector3 highestPoint = hits[0].point;
            foreach (RaycastHit hit in hits)
            {
                GameObject hitObject = hit.collider.gameObject;
                /*if (_currentAbility == GetComponent<Lasso>() && GetComponent<Lasso>().LoopedObject)
                {
                    PuzzleCube hitParent = hitObject.transform.parent.GetComponent<PuzzleCube>();
                    
                    Lasso lasso = GetComponent<Lasso>();
                    
                    
                }*/
                if (hitObject == GetComponent<Lasso>().LoopedObject )
                {
                    //Debug.Log("Next hit");
                    //break;
                }
                else if (hit.point.y > highestPoint.y)
                {
                    highestPoint = hit.point;
                }
            }
            if (hits.Length > 0)
            {
                returnPos = highestPoint;
                returnPos.y += 0.1f;
                return returnPos;
            }
            else
            {
                Debug.LogWarning("No Raycast Hit");
                return transform.position;
            }
        }
        public void SetRayCastPosition(Vector3 position)
        {
            float originalY = position.y;
            position.y = _raycastPoint.transform.position.y;
            _raycastPoint.transform.position = position;
            position.y = originalY - 1;
            _cursor.transform.position = position;
        }
    }
}
