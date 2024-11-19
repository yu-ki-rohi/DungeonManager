using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed = 1.0f;
    [SerializeField] private MonsterPool _monsterPool;
    private bool _isMovingCamera = false;
    private Vector2 _position = Vector2.zero;
    private Vector2 _delta = Vector2.zero;
    private UIButton _uiButton;

    public void OnPress(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RaycastHit2D hit = CheckHitUI();
            if (hit)
            {
                if(hit.collider.TryGetComponent<UIButton>(out var uiButton))
                {
                    _uiButton = uiButton.PressBehave();
                }
                return;
            }
            _isMovingCamera = true;
        }
        else if(context.canceled)
        {
            RaycastHit2D hit = CheckHitUI();
            _isMovingCamera = false;
            if(_uiButton != null)
            {
                int index = _uiButton.ReleaseBehave();
                Vector3 position = Camera.main.ScreenToWorldPoint(_position);
                position.z = 0.0f;
                Debug.Log(position);
                _monsterPool.Get(index, position);
                _uiButton = null;
            }
            if (hit)
            {
                if (hit.collider.TryGetComponent<UIButton>(out var uiButton))
                {
                    uiButton.ReleaseBehave();
                }
                return;
            }
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

    private RaycastHit2D CheckHitUI()
    {
        Ray ray = Camera.main.ScreenPointToRay(_position);
        int layerNum = LayerMask.NameToLayer("UI");
        int layerMask = 1 << layerNum;
        return Physics2D.Raycast(_position, (Vector2)ray.direction, 20.0f, layerMask);
    }

    private Vector3 Vec2ToVec3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
