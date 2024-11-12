using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed = 1.0f;
    private bool _isMovingCamera = false;
    private Vector2 _position = Vector2.zero;
    private Vector2 _delta = Vector2.zero;

    public void OnPress(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _isMovingCamera = true;
        }
        else if(context.canceled)
        {
            _isMovingCamera = false;
        }
    }

    public void OnPosition(InputAction.CallbackContext context)
    {
        _position = context.ReadValue<Vector2>();
    }

    public void OnDelta(InputAction.CallbackContext context)
    {
        _delta = context.ReadValue<Vector2>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMovingCamera)
        {
            Camera.main.transform.position -= new Vector3(_delta.x, _delta.y, 0) * Time.deltaTime * _cameraSpeed;
        }
        Debug.Log(_position);
    }
}
