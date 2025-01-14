using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : CharacterBase
{
    private StatusOfExplorer _optionStatus;
    private float _stamina;
    private int _money;
    private float _satisfaction = -1.0f;
    private float _rank = 1.0f;
    public void Initialize(Signpost destination, Signpost before, ExplorerData charaData, Vector3 dir, ObjectPoolBase pool, float riskLevel = 1.0f)
    {
        base.Initialize(destination,before, charaData, dir, pool);
        _optionStatus = charaData.OptionStatus;
        _rank += riskLevel / 10.0f;
        _stamina = _optionStatus.Stamina;
        _money = (int)(_optionStatus.Money * _rank);
    }

    protected override void Action()
    {
        base.Action();
        if (!IsReturn)
        {
            // EHP‚ªˆê’è’l‚ğ‰º‰ñ‚é
            // EƒXƒ^ƒ~ƒi‚ªs‚«‚é
            // ˆÈã‚¢‚¸‚ê‚©‚ÌğŒ‚ğ–‚½‚µ‚½‚ç“P‘Şó‘Ô‚Ö‘JˆÚ
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
            Attack((int)(_charaData.Status.Attack * _rank));
        }
        else
        {
            // “P‘Şó‘Ô‚ÌÛ‚Í“¦‘–‚ğ‚İ‚é
            if(Escape(Direction))
            {
                FinishBattle();
            }
            else
            {
                Attack((int)(_charaData.Status.Attack * _rank));
            }
        }
    }

    protected override void Die(int[] intData = null, float[] floatData = null)
    {
        intData = new int[1] { _money };
        floatData = new float[1] { _rank / 10.0f };
        base.Die(intData, floatData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            // Ú“G‚Ì’Ê’m‚Í’TõÒ‘¤‚©‚çs‚¤
            if (collision.TryGetComponent<CharacterBase>(out var charaBase))
            {
                charaBase.AddOpponent(this);
                AddOpponent(charaBase);
            }
        }
        else if (collision.tag == "TreasureBox")
        {
            if (collision.TryGetComponent<TreasureBox>(out var treasureBox))
            {
                int value = treasureBox.GetTreasure();
                _money += value / 2;
                _satisfaction += (
                    value - _optionStatus.ExpectedValue * _rank) / 100.0f
                    * (2.0f - _stamina / _optionStatus.Stamina)
                    * (2.0f - CurrentHp / _charaData.Status.MaxHp)
                    * _rank;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Entrance")
        {
            if (IsReturn)
            {
                float[] floatData = new float[2] { _satisfaction , _rank * 10.0f};
                DisAppear(0, null, floatData);
            }
        }
    }
}
