using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// 入力・レスポンスを管理
/// </summary>
public class ActionManager : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed = 1.0f;
    [SerializeField] private MonsterPool _monsterPool;
    [SerializeField] private GameObject _monsterImage;
    private bool _isMovingCamera = false;
    private Vector2 _position = Vector2.zero;
    private Vector2 _delta = Vector2.zero;
    private UIButton _uiButton;
    private Image _image;

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

                    //仮置き部分
                    _image.enabled = true;
                    //
                }
                return;
            }
            _isMovingCamera = true;
        }
        else if(context.canceled)
        {
            RaycastHit2D hit = CheckHitUI();
            _isMovingCamera = false;

            if (hit)
            {
                if (hit.collider.TryGetComponent<UIButton>(out var uiButton))
                {
                    uiButton.ReleaseInBehave();

                    //仮置き部分
                    _image.enabled = false;
                    //
                }
            }
            else if (_uiButton != null)
            {
                int index = _uiButton.ReleaseOutBehave();
                Vector3 position = Camera.main.ScreenToWorldPoint(_position);
                position.z = 0.0f;
                _monsterPool.Get(index, position);

                //仮置き部分
                _image.enabled = false;
                //
            }
            _uiButton = null;
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
        if(_monsterImage != null)
        {
            _image = _monsterImage.GetComponent<Image>();
            _image.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _monsterImage.transform.position = _position;
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
