using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PCamera : MonoBehaviour
    {
        GameObject _player;
        GameObject _pivotPoint;
        public GameObject PivotPoint => _pivotPoint;
        Vector3 _offset;
        
        private InputAction _rotateCameraLeft;
        private InputAction _rotateCameraRight;
        private InputAction _rotateCamera;
        private InputAction _moveCamera;
        [SerializeField] private float rotationSpeed = 50f;
        
        [Tooltip("xAxisLimits is the rotational limits for the x-axis of the _pivotPoint. The x value is the minimum and the y value is the maximum.")]
        [SerializeField] private Vector2 xAxisLimits;

        void Start()
        {
            _player = GameObject.FindFirstObjectByType<Player.Controller>().gameObject;
            _player.GetComponent<Controller>().PlayerCamera = this;
            _pivotPoint = transform.parent.gameObject;
            _offset = transform.position - _player.transform.position;
            
            _rotateCameraLeft = InputSystem.actions.FindAction("Rotate Camera Left");
            _rotateCameraRight = InputSystem.actions.FindAction("Rotate Camera Right");
            _rotateCamera = InputSystem.actions.FindAction("Rotate Camera");
            _moveCamera = InputSystem.actions.FindAction("Cursor Movement");
            Debug.Log("Limits:" + xAxisLimits);
        }
        private void Update()
        {
            _pivotPoint.transform.position = _player.transform.position;
        }

        private void LateUpdate()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            /*if (_rotateCameraLeft.IsPressed() && _rotateCameraRight.IsPressed())
            {
                return;
            }
            else if (_rotateCameraLeft.IsPressed())
            {
                _pivotPoint.transform.RotateAround(_pivotPoint.transform.position, _pivotPoint.transform.up, -rotationSpeed * Time.deltaTime);
            }
            else if (_rotateCameraRight.IsPressed())
            {
                _pivotPoint.transform.RotateAround(_pivotPoint.transform.position, _pivotPoint.transform.up, rotationSpeed * Time.deltaTime);
            }*/
            if (_rotateCamera.IsPressed())
            {
                Vector3 cameraRotationDelta = _moveCamera.ReadValue<Vector2>() * (rotationSpeed * Time.deltaTime);
                Vector3 newCameraRotation = _pivotPoint.transform.rotation.eulerAngles;
                
                newCameraRotation.x += cameraRotationDelta.y;
                newCameraRotation.y += cameraRotationDelta.x;
                
                /* For if we ever want to let the player rotate the x-axis of the pivot. Right now cant go below 0 
                newCameraRotation.x = (_pivotPoint.transform.rotation.eulerAngles.x > 180) ? _pivotPoint.transform.rotation.eulerAngles.x - 360 : _pivotPoint.transform.rotation.eulerAngles.x;
                */

                newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, 0, xAxisLimits.y);

                _pivotPoint.transform.localEulerAngles = newCameraRotation;
            }
        }
    }
}
