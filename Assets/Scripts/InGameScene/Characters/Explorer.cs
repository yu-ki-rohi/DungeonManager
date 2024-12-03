using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : CharacterBase
{
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
    }
}
