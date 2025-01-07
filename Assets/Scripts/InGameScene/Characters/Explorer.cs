using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : CharacterBase
{
    private float _stamina;
    private int _money;
    private float _satisfaction = 0.0f;
    public void Initialize(Signpost destination, Signpost before, ExplorerData charaData, Vector3 dir, ObjectPoolBase pool)
    {
        base.Initialize(destination,before, charaData, dir, pool);
        _stamina = charaData.OptionStatus.Stamina;
        _money = charaData.OptionStatus.Money;
    }

    protected override void Action()
    {
        base.Action();
        if (!IsReturn)
        {
            // �EHP�����l�������
            // �E�X�^�~�i���s����
            // �ȏア���ꂩ�̏����𖞂�������P�ޏ�Ԃ֑J��
            if (CurrentHp < CharaData.Status.MaxHp / 2)
            {
                ReturnToEntrance();
            }
            else if (_stamina > 0)
            {
                _stamina -= Time.deltaTime;
            }
            else
            {
                ReturnToEntrance();
            }
        }
        
    }

    protected override void Battle()
    {
        if(!IsReturn)
        {
            base.Battle();
        }
        else
        {
            // �P�ޏ�Ԃ̍ۂ͓��������݂�
            if(Escape(Direction))
            {
                FinishBattle();
            }
            else
            {
                base.Battle();
            }
        }
    }

    protected override void Die(int[] intData = null, float[] floatData = null)
    {
        intData = new int[1] { _money };
        base.Die(intData, null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            // �ړG�̒ʒm�͒T���ґ�����s��
            if (collision.TryGetComponent<CharacterBase>(out var charaBase))
            {
                charaBase.AddOpponent(this);
                AddOpponent(charaBase);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Entrance")
        {
            if (IsReturn)
            {
                float[] floatData = new float[1] { _satisfaction };
                DisAppear(0, null, floatData);
            }
        }
    }
}
