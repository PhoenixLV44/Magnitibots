using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingCursor : MonoBehaviour
{
    private GameObject _targetCursor;
    private Vector3 _playerPosition;
    [SerializeField] private float cursorSpeed = 5f;
    private void Awake()
    {
        _targetCursor = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule),transform.position,transform.rotation );
        _targetCursor.transform.localScale = new Vector3(1.25f, 0.5f, 1.25f);
        _targetCursor.transform.parent = transform;
        _targetCursor.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, _targetCursor.transform.position.z);
        _targetCursor.GetComponent<CapsuleCollider>().enabled = false;
        _targetCursor.name = "Target Cursor";
        _targetCursor.SetActive(false);
    }

    public void ActivateCursor(Vector3 position)
    {
        _targetCursor.transform.position = position;
        Cursor.lockState = CursorLockMode.Locked;
        _targetCursor.SetActive(true);
    }
    public void DeactivateCursor()
    {
        _targetCursor.transform.position = transform.position;
        Cursor.lockState = CursorLockMode.None;
        _targetCursor.SetActive(false);
    }

    public Vector3 MoveCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 cursorMovement = InputSystem.actions.FindAction("Cursor Movement").ReadValue<Vector2>();

        cursorMovement.z = cursorMovement.y;
        cursorMovement.y = 0;
        //Debug.Log(cursorMovement);
        //_targetCursor.transform.position = new Vector3(transform.position.x + ( cursorMovement.x * cursorSpeed * Time.deltaTime), transform.position.y, _targetCursor.transform.position.z +  ( cursorMovement.y * cursorSpeed * Time.deltaTime));
        _targetCursor.transform.position += cursorMovement * Time.deltaTime * cursorSpeed;

        return _targetCursor.transform.position;
    }
}
