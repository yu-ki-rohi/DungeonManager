using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    
    // 
    private bool _isReturn = false;
    private SpriteRenderer _spriteRenderer;
    // 目的地
    private Signpost _destination;
    private Signpost _beforeSignpost;
    private Vector3 _dir = Vector3.zero;

    private ObjectPoolBase _pool;
    private CharaData _charaData;   // いずれ取得のみに制限できるようにしたい

    private List<CharacterBase> _opponentList = new List<CharacterBase>();

    private int _currentHp;
    private float _attackTimer;

    public Signpost Destination { get { return _destination; } }
    public Signpost BeforeSingPost { get { return _beforeSignpost; } }

    public int CurrentHp { get { return _currentHp; } }
    public CharaData CharaData { get { return _charaData;} }
    public bool IsReturn { get { return _isReturn; } }

    public void Initialize(Signpost destination, Signpost before, CharaData charaData, Vector3 dir, ObjectPoolBase pool)
    {
        _destination = destination;
        _beforeSignpost = before;
        _charaData = charaData;
        _dir = dir;
        if(_spriteRenderer != null)
        {
            _spriteRenderer.sprite = _charaData.Info.Sprite;
        }
        _pool = pool;

        // TODO:その他初期化処理
        _currentHp = _charaData.Status.MaxHp;
        _attackTimer = _charaData.Status.CoolTime;
        _isReturn = false;
    }

    public void AddOpponent(CharacterBase opponent)
    {
        _opponentList.Add(opponent);
    }

    public void RemoveOpponent(CharacterBase opponent)
    {
        _opponentList.Remove(opponent);
    }

    public void ClearOpponent()
    {
        _opponentList.Clear();
    }

    public void Damage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Action()
    {
        if (_opponentList.Count > 0)
        {
            if (_attackTimer < 0)
            {
                Battle();
            }
            else
            {
                _attackTimer -= Time.deltaTime;
            }
        }
        else
        {
            Move();
        }
    }

    protected virtual void Battle()
    {
        _opponentList[0].Damage(_charaData.Status.Attack);
        _attackTimer = _charaData.Status.CoolTime;
    }

    protected void Escape()
    {
        foreach (var opponent in _opponentList)
        {
            opponent.RemoveOpponent(this);
        }
        ClearOpponent();
    }

    protected void DisAppear()
    {
        Escape();
        _pool.Release(gameObject);
    }

    protected virtual void Move()
    {
        if (_destination != null)
        {
            Vector3 dir = _destination.transform.position - transform.position;
            if ((transform.position - _destination.transform.position).sqrMagnitude < 0.04f ||
                Vector3.Dot(_dir, dir) < 0)
            {
                Signpost currentSignpost = _destination;
                if (_isReturn)
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
        transform.position += _dir * _charaData.Status.Speed * Time.deltaTime;
#else
            }
        }
        // 処理速度への影響を確認する用
        transform.position += (_destination.transform.position - transform.position).normalized * _characterList.Get(0).Status.Speed * Time.deltaTime;
#endif
    }

    protected void ReturnToEntrance()
    {
        _isReturn = true;
        if(_destination.Depth > _beforeSignpost.Depth)
        {
            _dir *= -1;
            Signpost tmp = _destination;
            _destination = _beforeSignpost;
            _beforeSignpost = tmp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _charaData.Info.Sprite;
        _dir = (_destination.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Action();
    }

    private void Die()
    {
        // 以下死亡時処理追加

        //
        DisAppear();
    }
}
