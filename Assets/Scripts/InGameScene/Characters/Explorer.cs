using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : CharacterBase
{
    private float _stamina;
    private float _satisfaction = 0.0f;
    public void Initialize(Signpost destination, Signpost before, ExplorerData charaData, Vector3 dir, ObjectPoolBase pool)
    {
        base.Initialize(destination,before, charaData, dir, pool);
        _stamina = charaData.OptionStatus.StaminaMax;
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
            base.Battle();
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
                base.Battle();
            }
        }
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Entrance")
        {
            if (IsReturn)
            {
                DisAppear();
            }
        }
    }
}
