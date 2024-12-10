using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : CharacterBase
{
    private float _stamina;
    public void Initialize(Signpost destination, Signpost before, ExplorerData charaData, Vector3 dir, ObjectPoolBase pool)
    {
        base.Initialize(destination,before, (CharaData)charaData, dir, pool);
        _stamina = charaData.OptionStatus.StaminaMax;
    }

    protected override void Action()
    {
        base.Action();
        if (!IsReturn)
        {
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
            Escape();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            if (collision.TryGetComponent<CharacterBase>(out var charaBase))
            {
                charaBase.AddOpponent(this);
                AddOpponent(charaBase);
            }
        }
        else if(collision.tag == "Entrance")
        {
            if(IsReturn)
            {
                DisAppear();
            }
        }
    }
}
