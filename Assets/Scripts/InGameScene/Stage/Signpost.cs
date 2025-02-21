using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour
{
    // フィールド
    [SerializeField] private Signpost[] _adjacentSignposts;
    [SerializeField] private bool _isRoom = false;
    private int _value = -1;
    private float _depth = -1.0f;
    // 撤退時に向かうべき方向を示すインデックス
    private int _returnIndex = -1;

    // プロパティ
    public int Value { get { return _value; } }
    public float Depth { get { return _depth; } }

    // Editor表示用


    // 公開メソッド



    public void SetValue(int value)
    {
        _value = value + 1;
        for (int i = 0;  i < _adjacentSignposts.Length; i++)
        {
            if (_adjacentSignposts[i].Value < 0 || _adjacentSignposts[i].Value > _value + 1)
            {
                _adjacentSignposts[i].SetValue(_value);
            }
        }
    }

    /// <summary>
    /// その道しるべが入口がどれだけ離れているか(深度)を設定
    /// </summary>
    /// <param name="depth">呼び出し元の深度</param>
    /// <param name="signpost">呼び出し元の道しるべ</param>
    public void SetDepth(float depth,Signpost signpost = null)
    {
        // 入口以外では、呼び出し元に向けた
        if(depth != 0.0f)
        {
            bool isfind = false;
            for (int i = 0;i < _adjacentSignposts.Length;i++)
            {
                if(signpost == _adjacentSignposts[i])
                {
                    _returnIndex = i;
                    isfind = true;
                    break;
                }
            }

            if(!isfind)
            {
                return;
            }
        }

        _depth = depth;
        for (int i = 0; i < _adjacentSignposts.Length; i++)
        {
            float distance = (_adjacentSignposts[i].transform.position - transform.position).magnitude;
            if (_adjacentSignposts[i].Depth < 0 || _adjacentSignposts[i].Depth > Depth + distance)
            {

                _adjacentSignposts[i].SetDepth(Depth + distance, this);
            }
        }
    }


#if true
    // 方向情報を事前に用意したパターン
    // 追加フィールド
    private Vector3[] _directs = null;
    // 公開メソッド
    public Signpost GetNextDestination(out Vector3 dir, Signpost beforeSignpost = null)
    {
        // どこにも向かえない場合、自身を返す
        if (_adjacentSignposts.Length <= 0)
        {
            dir = Vector3.zero;
            return this;
        }
        // 行き先が一つの場合は、そこを返す
        else if (_adjacentSignposts.Length == 1)
        {
            dir = _directs[0];
            return _adjacentSignposts[0];
        }

        // 直前の場所を引き数で渡された場合、そこ以外を返す
        int judge;
        do
        {
            judge = Random.Range(0, _adjacentSignposts.Length);
        } while (_adjacentSignposts[judge] == beforeSignpost);
        dir = _directs[judge];
        return _adjacentSignposts[judge];
    }

    public Signpost GetReturnDestination(out Vector3 dir)
    {
        if (_value == 0)
        {
            dir = Vector3.zero;
            return this;
        }
        if (_returnIndex < 0 || _returnIndex >= _adjacentSignposts.Length)
        {
            dir = Vector3.zero;
            return this;
        }
        dir = _directs[_returnIndex];
        return _adjacentSignposts[_returnIndex];
    }

    public Signpost GetApperDestination(out Vector3 dir, out Vector3 apperPosition, Vector3 position)
    {
        dir = Vector3.zero;
        apperPosition = this.transform.position;
        Signpost destination = this;
        if (_adjacentSignposts.Length <= 0)
        {
            return destination;
        }


        // 最もなす角が小さい方向を目的地に
        float dot = -2.0f;
        Vector3 toInputPositionNormalized = (position - this.transform.position).normalized;
        for (int i = 0; i < _adjacentSignposts.Length; i++)
        {
            float tmpDot = Vector3.Dot(toInputPositionNormalized, _directs[i]);
            if (dot < tmpDot)
            {
                dot = tmpDot;
                dir = _directs[i];
                destination = _adjacentSignposts[i];
            }
        }

        // 出現座標の設定
        if (dot > 0)
        {
            apperPosition = (position - this.transform.position).magnitude * dot * dir + this.transform.position;
        }
        return destination;
    }

    void Start()
    {
        _directs = new Vector3[_adjacentSignposts.Length];
        //_distances = new float[_adjacentSignposts.Length];
        for (int i = 0; i < _adjacentSignposts.Length; i++)
        {
            _directs[i] = (_adjacentSignposts[i].transform.position - transform.position).normalized;
        }
    }
#else
    // 方向情報を事前に用意しないパターン
    public Signpost GetNextDestination(Signpost beforeSignpost = null)
    {
        // どこにも向かえない場合、自身を返す
        if (_adjacentSignposts.Length <= 0)
        {
            return this;
        }
        // 行き先が一つの場合は、そこを返す
        else if (_adjacentSignposts.Length == 1)
        {
            return _adjacentSignposts[0];
        }

        // 直前の場所を引き数で渡された場合、そこ以外を返す
        int judge;
        do
        {
            judge = Random.Range(0, _adjacentSignposts.Length);
        } while (_adjacentSignposts[judge] == beforeSignpost);
        return _adjacentSignposts[judge];
    }

    public Signpost GetReturnDestination()
    {
        Signpost target = this;
        for (int i = 0; i < _adjacentSignposts.Length; i++)
        {
            if (target.Depth > _adjacentSignposts[i].Depth)
            {
                target = _adjacentSignposts[i];
            }
        }
        return target;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
#endif

    // 非公開メソッド

    // Editor表示用
    private void OnDrawGizmos()
    {
        for(int i = 0; i < _adjacentSignposts.Length; i++)
        {
            if (_adjacentSignposts[i] != null)
            {
                Gizmos.DrawLine(transform.position, _adjacentSignposts[i].transform.position);

                // 矢じりの描画
                float ratio = 0.3f;
                Vector3 vec = _adjacentSignposts[i].transform.position - transform.position;
                float scale = 0.5f;
                Gizmos.DrawLine(
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position,
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position +
                    (Vector3.Cross(vec, Vector3.forward) - vec).normalized * scale);
                Gizmos.DrawLine(
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position,
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position -
                    (Vector3.Cross(vec, Vector3.forward) + vec).normalized * scale);
            }
        }
    }


}
