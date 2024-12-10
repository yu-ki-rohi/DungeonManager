using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    private int _targetIndex = 0;

    public Signpost Destination { get { return _destination; } }
    public Signpost BeforeSingPost { get { return _beforeSignpost; } }

    public int CurrentHp { get { return _currentHp; } }
    public CharaData CharaData { get { return _charaData;} }
    public bool IsReturn { get { return _isReturn; } }
    public Vector2 Direction { get { return _dir; } }

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
        _targetIndex = 0;
    }

    public void AddOpponent(CharacterBase opponent)
    {
        _opponentList.Add(opponent);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="opponent"></param>
    /// <param name="other">逃げて消失した際に追わせる場合に使用</param>
    public void RemoveOpponent(CharacterBase opponent, Transform other = null)
    {
        _targetIndex = 0;
        _opponentList.Remove(opponent);
        if(_opponentList.Count == 0 && other != null)
        {
            Vector3 toOpponent = other.position - transform.position;
            if(Vector3.Dot(toOpponent, Direction) < 0)
            {
                TurnBack();
            }
        }
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
                _attackTimer = _charaData.Status.CoolTime;
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
        if(_targetIndex < _opponentList.Count)
        {
            _opponentList[_targetIndex].Damage(_charaData.Status.Attack);
        }
        else
        {
            _opponentList[0].Damage(_charaData.Status.Attack);
        }
    }

    protected bool Escape(Vector3 dir)
    {
        for(int i = 0;  i < _opponentList.Count; i++)
        {
            Vector3 toOpponent = _opponentList[i].gameObject.transform.position - transform.position;
            if (Vector3.Dot(toOpponent, dir) > 0 || _opponentList[i].CharaData.Status.Speed > CharaData.Status.Speed)
            {
                _targetIndex = i;
                return false;
            }
        }
        foreach (var opponent in _opponentList)
        {
           
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isEscape">逃げたかどうか</param>
    protected void FinishBattle(bool isEscape = false)
    {
        Transform transform = null;
        if(isEscape)
        {
            transform = this.transform;
        }

        foreach (var opponent in _opponentList)
        {
            opponent.RemoveOpponent(this,transform);
        }
        ClearOpponent();
        _targetIndex = 0;
    }
    protected void DisAppear()
    {
        FinishBattle();
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
            TurnBack();
        }
    }

    protected void TurnBack()
    {
        _dir *= -1;
        Signpost tmp = _destination;
        _destination = _beforeSignpost;
        _beforeSignpost = tmp;
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
