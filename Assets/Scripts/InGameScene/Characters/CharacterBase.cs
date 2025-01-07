using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // �P�ޏ�Ԃ��ǂ����E�v���C���[�̂�?
    private bool _isReturn = false;
    // �ڕW�ƂȂ铹�����
    private Signpost _destination;
    // ���O�ɒʉ߂����������
    private Signpost _beforeSignpost;
    // �ړ�����
    private Vector3 _dir = Vector3.zero;
    // ���g���Ǘ����Ă���v�[��
    private ObjectPoolBase _pool;
    // �Q�Ƃ���f�[�^
    private CharaData _charaData;   // ������擾�݂̂ɐ����ł���悤�ɂ�����
    // �퓬����̃��X�g
    private List<CharacterBase> _opponentList = new List<CharacterBase>();

    private int _currentHp;
    private Timer _attackTimer;
    private int _targetIndex = 0;

    // �v���p�e�B
    public Signpost Destination { get { return _destination; } }
    public Signpost BeforeSingPost { get { return _beforeSignpost; } }

    public int CurrentHp { get { return _currentHp; } }
    public CharaData CharaData { get { return _charaData;} }
    public bool IsReturn { get { return _isReturn; } }
    public Vector2 Direction { get { return _dir; } }

    // ���J���\�b�h
    // ������
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

        // TODO:���̑�����������
        _currentHp = _charaData.Status.MaxHp;
        _attackTimer = new Timer(Battle, _charaData.Status.CoolTime);
        _isReturn = false;
        _targetIndex = 0;
    }

    /// <summary>
    /// �U���Ώۂ̒ǉ�
    /// </summary>
    /// <param name="opponent">�ǉ����鑊��</param>
    public void AddOpponent(CharacterBase opponent)
    {
        _opponentList.Add(opponent);
    }

    /// <summary>
    /// �U���Ώۂ̍폜
    /// </summary>
    /// <param name="opponent">�폜���鑊��</param>
    /// <param name="other">�����ď��������ۂɒǂ킹��ꍇ�Ɏg�p</param>
    public void RemoveOpponent(CharacterBase opponent, Transform other = null)
    {
        _targetIndex = 0;
        _opponentList.Remove(opponent);

        // �Ō�̑��肪�������ꍇ�A�ǂ�������
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
    /// �U���Ώۂ�S�폜
    /// </summary>
    public void ClearOpponent()
    {
        _opponentList.Clear();
    }

    /// <summary>
    /// �_���[�W���󂯂郁�\�b�h
    /// </summary>
    /// <param name="damage">�󂯂�_���[�W</param>
    public void Damage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// �L�����N�^�[�̍s��
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
    /// �퓬���̍s��
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
    /// �����\���ǂ����𔻒f
    /// </summary>
    /// <param name="dir">��������</param>
    /// <returns>�������\��</returns>
    protected bool Escape(Vector3 dir)
    {
        for(int i = 0;  i < _opponentList.Count; i++)
        {
            Vector3 toOpponent = _opponentList[i].gameObject.transform.position - transform.position;
            if (Vector3.Dot(toOpponent, dir) > 0 || _opponentList[i].CharaData.Status.Speed > CharaData.Status.Speed)
            {
                // �E�i�s�����ɓG������
                // �E�������ړ����x�������G������
                // ��L�����ꂩ�𖞂����ꍇ�A�������s and ���̓G�����b�N�I��
                _targetIndex = i;
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// �퓬�I��
    /// </summary>
    /// <param name="isEscape">���������ǂ���</param>
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
    /// �ړ����\�b�h
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
                // �����]�����A���g�Ō��������߂�ꍇ
                _dir = (_destination.transform.position - transform.position).normalized;
#endif
            }
        }
        transform.position += _dir * _charaData.Status.Speed * Time.deltaTime;
#else
            }
        }
        // �������x�ւ̉e�����m�F����p
        transform.position += (_destination.transform.position - transform.position).normalized * _characterList.Get(0).Status.Speed * Time.deltaTime;
#endif
    }

    /// <summary>
    /// ���g���V�[������ޏꂷ��ۂɌĂяo��
    /// </summary>
    protected void DisAppear(int index = 0, int[] intData = null, float[] floatData = null)
    {
        FinishBattle();
        _pool.Release(gameObject, index, intData, floatData);
    }

    /// <summary>
    /// �P�ޏ�Ԃֈڍs���郁�\�b�h
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
    /// �ړ������̔��]
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
    /// �L�����N�^�[���|���ꂽ�ۂ̃��\�b�h
    /// </summary>
    protected virtual void Die(int[] intData = null, float[] floatData = null)
    {
        // �ȉ��ɓ|���ꂽ�ۂ̏���

        //
        DisAppear(1, intData, floatData);
    }
}
