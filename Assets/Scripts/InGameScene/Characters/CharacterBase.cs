using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private Signpost _destination;
    [SerializeField] private CharacterList _characterList;
    [SerializeField] private bool _isReturn = false;
    private SpriteRenderer _spriteRenderer;
    private Signpost _beforeSignpost;
    private int _index = 0;
    private Vector3 _dir = Vector3.zero;

    public void Initialize(Signpost destination, CharacterList characterList, int index, Vector3 dir)
    {
        _destination = destination;
        _characterList = characterList;
        _index = index;
        _dir = dir;
        if(_spriteRenderer != null)
        {
            _spriteRenderer.sprite = _characterList.Get(0).Info.Sprite;
        }

        // TODO:その他初期化処理

    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _characterList.Get(_index).Info.Sprite;
        _dir = (_destination.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(_destination != null)
        {
            Vector3 dir = _destination.transform.position - transform.position;
            if ((transform.position - _destination.transform.position).sqrMagnitude < 0.04f ||
                Vector3.Dot(_dir,dir) < 0)
            {
                Signpost currentSignpost = _destination;
                if(_isReturn)
                {
                    _destination = _destination.GetReturnDestination(out _dir);
                }
                else
                {
#if false
                    _destination = _destination.GetNextDestination(_beforeSignpost);
#else
                    _destination = _destination.GetNextDestination(out _dir, _beforeSignpost);
#endif
                }
                _beforeSignpost = currentSignpost;
#if true
#if false
                // 方向転換時、自身で向きを決める場合
                _dir = (_destination.transform.position - transform.position).normalized;
#endif
            }
        }
        transform.position += _dir * _characterList.Get(_index).Status.Speed * Time.deltaTime;
#else
            }
        }
        // 処理速度への影響を確認する用
        transform.position += (_destination.transform.position - transform.position).normalized * _characterList.Get(0).Status.Speed * Time.deltaTime;
#endif
    }
}
