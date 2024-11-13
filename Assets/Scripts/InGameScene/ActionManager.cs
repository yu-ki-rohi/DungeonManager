using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed = 1.0f;
    [SerializeField] private RectTransform _rectTransform;
    private bool _isMovingCamera = false;
    private Vector2 _position = Vector2.zero;
    private Vector2 _delta = Vector2.zero;


    public void OnPress(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(_position);
            RaycastHit2D hit;
            int layerNum = LayerMask.NameToLayer("UI");
            int layerMask = 1 << layerNum;
            hit = Physics2D.Raycast(_position, (Vector2)ray.direction, 20.0f, layerMask);
            if (hit)
            {
                Debug.Log("Hit!!");
                return;
            }
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
        //Debug.Log(Camera.main.ScreenToWorldPoint(_position));
    }

    private Vector3 Vec2ToVec3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
