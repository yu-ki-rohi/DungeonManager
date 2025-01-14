using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// 入力・レスポンスを管理
/// </summary>
public class ActionManager : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed = 1.0f;
    [SerializeField] private MonsterPool _monsterPool;
    [SerializeField] private GameObject _monsterImage;
    [SerializeField] private InGameManager _gameManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ItemList _itemList;
    private bool _isMovingCamera = false;
    private Vector2 _position = Vector2.zero;
    private Vector2 _delta = Vector2.zero;
    private UIButton _uiButton;
    private Image _image;


    public void OnPress(InputAction.CallbackContext context)
    {
        // 押した瞬間
        if(context.performed)
        {
            RaycastHit2D hit = CheckHitUI();
            if (hit)
            {
                if(hit.collider.TryGetComponent<UIButton>(out var uiButton))
                {
                    int index;
                    UIButton.UIType type;
                    _uiButton = uiButton.PressBehave(out type, out index);

                    switch(type)
                    {
                        case UIButton.UIType.MonsterRepository:
                            _image.sprite = _monsterPool.GetSprite(index);
                            break;
                        case UIButton.UIType.ItemRepository:
                            _image.sprite = _itemList.Get(index).Info.Sprite;
                            break;
                    }

                    //仮置き部分
                    _image.enabled = true;
                    //
                }
                return;
            }
            _isMovingCamera = true;
        }
        // 離した瞬間
        else if(context.canceled)
        {
            _isMovingCamera = false;
            if (_uiButton != null)
            {
                RaycastHit2D hit = CheckHitUI();
                if (hit)
                {
                    if (hit.collider.TryGetComponent<UIButton>(out var uiButton))
                    {
                        int index;
                        uiButton.ReleaseInBehave(out index);

                        //仮置き部分
                        _image.enabled = false;
                        //
                    }
                }
                else
                {
                    int index;
                    UIButton.UIType uiType = _uiButton.ReleaseOutBehave(out index);
                    switch(uiType)
                    {
                        case UIButton.UIType.MonsterRepository:
                            // モンスター出現処理
                            SummonMonster(index);
                            break;
                        case UIButton.UIType.ItemRepository:
                            SetItem(index);
                            break;
                    }

                    //仮置き部分
                    _image.enabled = false;
                    //
                }
                _uiButton = null;
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

    private RaycastHit2D CheckHitObject(string layerName)
    {
        Ray ray = Camera.main.ScreenPointToRay(_position);
        int layerNum = LayerMask.NameToLayer(layerName);
        int layerMask = 1 << layerNum;
        return Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 20.0f, layerMask);
    }

    private bool SummonMonster(int index)
    {
        if(!_gameManager.CanPurchase(_monsterPool.GetCost(index)))
        {
            return false;
        }
        Vector3 position = Camera.main.ScreenToWorldPoint(_position);
        position.z = 0.0f;
        _monsterPool.Get(index, position);

        return true;

    }

    private void SetItem(int index)
    {
        RaycastHit2D hit = CheckHitObject("TreasureBox");
        if(hit)
        {
            TreasureBox treasureBox = hit.collider.GetComponent<TreasureBox>();
            if(treasureBox == null || treasureBox.IsClosed())
            {
                return;
            }

            int value = _itemList.Get(index).Status.Value;
            if(_gameManager.CanPurchase(value))
            {
                treasureBox.SetTreasure(value);
            }

        }
    }
}
