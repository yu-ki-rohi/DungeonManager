using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterPool : CharacterPool
{
    [SerializeField] private MonsterList _monsterList;

    private Signpost[] _signposts;

    public GameObject Get(int id, Vector3 position)
    {
        if (_signposts.Length <= 0)
        {
            Debug.LogAssertion("Initialize MonsterPool !!");
            return null;
        }

        // idが範囲外のときはnullを返す
        if (id < 0 || id >= _monsterList.List.Count)
        {
            return null;
        }

        // 余裕ができたらアルゴリズムを変更

        // 出現位置の根本側を決定
        float distance = -1.0f;
        Signpost apperSignpost = null;
        foreach (var signpost in _signposts)
        {
            if(apperSignpost == null || distance > (signpost.transform.position - position).sqrMagnitude)
            {
                distance = (signpost.transform.position - position).sqrMagnitude;
                apperSignpost = signpost;
            }
        }

        // 移動先や出現座標を決定
        if(apperSignpost != null)
        {
            Vector3 dir, appearPosition;
            Signpost destination = apperSignpost.GetApperDestination(out dir, out appearPosition, position);
            GameObject monster = Get(appearPosition);

            // TODO:方向、目的地とモンスター情報の初期化処理
            if(monster.TryGetComponent<CharacterBase>(out var charaBase))
            {
                charaBase.Initialize(destination, apperSignpost, _monsterList.Get(id), dir, this);
            }
            return monster;
        }
        return null;
    }

    public void Initialize(Transform signpostsParent)
    {
        if (signpostsParent != null)
        {
            // _signpostsに各Signpostコンポーネントを格納
            _signposts = new Signpost[signpostsParent.childCount];
            for (int i = 0; i < signpostsParent.childCount; i++)
            {
                _signposts[i] = signpostsParent.GetChild(i).GetComponent<Signpost>();
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
