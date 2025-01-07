using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // 撤退状態かどうか・プレイヤーのみ?
    private bool _isReturn = false;
    // 目標となる道しるべ
    private Signpost _destination;
    // 直前に通過した道しるべ
    private Signpost _beforeSignpost;
    // 移動方向
    private Vector3 _dir = Vector3.zero;
    // 自身を管理しているプール
    private ObjectPoolBase _pool;
    // 参照するデータ
    private CharaData _charaData;   // いずれ取得のみに制限できるようにしたい
    // 戦闘相手のリスト
    private List<CharacterBase> _opponentList = new List<CharacterBase>();

    private int _currentHp;
    private Timer _attackTimer;
    private int _targetIndex = 0;

    // プロパティ
    public Signpost Destination { get { return _destination; } }
    public Signpost BeforeSingPost { get { return _beforeSignpost; } }

    public int CurrentHp { get { return _currentHp; } }
    public CharaData CharaData { get { return _charaData;} }
    public bool IsReturn { get { return _isReturn; } }
    public Vector2 Direction { get { return _dir; } }

    // 公開メソッド
    // 初期化
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
        _attackTimer = new Timer(Battle, _charaData.Status.CoolTime);
        _isReturn = false;
        _targetIndex = 0;
    }

    /// <summary>
    /// 攻撃対象の追加
    /// </summary>
    /// <param name="opponent">追加する相手</param>
    public void AddOpponent(CharacterBase opponent)
    {
        _opponentList.Add(opponent);
    }

    /// <summary>
    /// 攻撃対象の削除
    /// </summary>
    /// <param name="opponent">削除する相手</param>
    /// <param name="other">逃げて消失した際に追わせる場合に使用</param>
    public void RemoveOpponent(CharacterBase opponent, Transform other = null)
    {
        _targetIndex = 0;
        _opponentList.Remove(opponent);

        // 最後の相手が逃げた場合、追いかける
        if(_opponentList.Count == 0 && other != null)
        {
            Vector3 toOpponent = other.position - transform.position;
            if(Vector3.Dot(toOpponent, Direction) < 0)
            {
                TurnBack();
            }
        }
    }

    /// <summary>
    /// 攻撃対象を全削除
    /// </summary>
    public void ClearOpponent()
    {
        _opponentList.Clear();
    }

    /// <summary>
    /// ダメージを受けるメソッド
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void Damage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// キャラクターの行動
    /// </summary>
    protected virtual void Action()
    {
        if (_opponentList.Count > 0)
        {
            _attackTimer.Update(Time.deltaTime);
        }
        else
        {
            Move();
        }
    }

    /// <summary>
    /// 戦闘時の行動
    /// </summary>
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

    /// <summary>
    /// 逃走可能かどうかを判断
    /// </summary>
    /// <param name="dir">逃走方向</param>
    /// <returns>逃走が可能か</returns>
    protected bool Escape(Vector3 dir)
    {
        for(int i = 0;  i < _opponentList.Count; i++)
        {
            Vector3 toOpponent = _opponentList[i].gameObject.transform.position - transform.position;
            if (Vector3.Dot(toOpponent, dir) > 0 || _opponentList[i].CharaData.Status.Speed > CharaData.Status.Speed)
            {
                // ・進行方向に敵がいる
                // ・自分より移動速度が速い敵がいる
                // 上記いずれかを満たす場合、逃走失敗 and その敵をロックオン
                _targetIndex = i;
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 戦闘終了
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


    /// <summary>
    /// 移動メソッド
    /// </summary>
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

    /// <summary>
    /// 自身がシーンから退場する際に呼び出す
    /// </summary>
    protected void DisAppear(int index = 0, int[] intData = null, float[] floatData = null)
    {
        FinishBattle();
        _pool.Release(gameObject, index, intData, floatData);
    }

    /// <summary>
    /// 撤退状態へ移行するメソッド
    /// </summary>
    protected void ReturnToEntrance()
    {
        _isReturn = true;
        if(_destination.Depth > _beforeSignpost.Depth)
        {
            TurnBack();
        }
    }

    /// <summary>
    /// 移動方向の反転
    /// </summary>
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

    /// <summary>
    /// キャラクターが倒された際のメソッド
    /// </summary>
    protected virtual void Die(int[] intData = null, float[] floatData = null)
    {
        // 以下に倒された際の処理

        //
        DisAppear(1, intData, floatData);
    }
}
