using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterPool : CharacterPool
{
    [SerializeField] private MonsterList _monsterList;

    private Signpost[] _signposts;

    public int GetCost(int index)
    {
        return _monsterList.GetOptionData(index).OptionStatus.Cost;
    }

    public Sprite GetSprite(int index)
    {
        return _monsterList.Get(index).Info.Sprite;
    }
    

    public GameObject Get(int id, Vector3 position)
    {
        if (_signposts.Length <= 0)
        {
            // �������s��
            // Dungeon��SignpostsParent���o�^����Ă��Ȃ�
            Debug.LogAssertion("Initialize MonsterPool !!");
            return null;
        }
        if (id < 0 || id >= _monsterList.List.Count)
        {
            // �A�N�Z�X�ᔽ
            Debug.LogAssertion("Illegal Access in Monster List !!");
            return null;
        }

        // �]�T������΃A���S���Y����ύX������

        // �o���ʒu�̍��{��������
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

        // �ړ����o�����W������
        if(apperSignpost != null)
        {
            Vector3 dir, appearPosition;
            Signpost destination = apperSignpost.GetApperDestination(out dir, out appearPosition, position);
            GameObject monster = Get(appearPosition);

            // TODO:�����A�ړI�n�ƃ����X�^�[���̏���������
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
            // _signposts�ɊeSignpost�R���|�[�l���g���i�[
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
