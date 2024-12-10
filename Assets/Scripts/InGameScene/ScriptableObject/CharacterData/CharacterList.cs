using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO ����:�C���^�[�t�F�[�X�𗘗p�����ق����ǂ��݌v�ɂȂ邩��?

[Serializable]
public class InfoOfCharacter
{
    public string Name;
    public Sprite Sprite;
}

[Serializable]
public class StatusOfCharacter
{
    public int MaxHp = 100;
    public int Attack = 10;
    public float CoolTime = 1.0f;
    public float Speed = 2.0f;
}

[Serializable]
public class CharaData
{
    public InfoOfCharacter Info;
    public StatusOfCharacter Status;
}

public class CharacterList : ScriptableObject
{
    public virtual CharaData Get(int index)
    {
        return null;
    }
}
