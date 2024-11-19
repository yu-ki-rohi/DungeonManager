using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : ObjectPoolBase
{
    [SerializeField] private Transform _signpostsParent;
    [SerializeField] private MonsterList _monsterList;
    private Signpost[] _signposts;

    public GameObject Get(int id, Vector3 position)
    {
        // idが範囲外のときはnullを返す
        if(id < 0 || id >= _monsterList.List.Count)
        {
            return null;
        }

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
            GameObject monster = base.Get(appearPosition);

            // TODO:方向、目的地とモンスター情報の初期化処理
            if(monster.TryGetComponent<CharacterBase>(out var charaBase))
            {
                charaBase.Initialize(destination, _monsterList, id, dir);
            }
        }
        return null;
    }

    protected override void Start()
    {
        if(_signpostsParent != null)
        {
            // _signpostsに各Signpostコンポーネントを格納
            _signposts = new Signpost[_signpostsParent.childCount];
            for(int i = 0;  i < _signpostsParent.childCount; i++)
            {
                _signposts[i] = _signpostsParent.GetChild(i).GetComponent<Signpost>();
            }
        }
        base.Start();
    }
}
