using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class StatusOfMonster
{
    public int Money = 10;
}

[Serializable]
public class MonsterData : CharaData
{
    public StatusOfMonster OptionStatus;
}

[CreateAssetMenu(menuName = "ScriptableObject/CharacterList/Monster", fileName = "MonsterList")]
public class MonsterList : CharacterList
{
    public List<MonsterData> List = new List<MonsterData>();

    public override CharaData Get(int index)
    {
        if(index < 0 || index >= List.Count)
        {

            return null;
        }
        return List[index];
    }

    public MonsterData GetOptionData(int index)
    {
        if (index < 0 || index >= List.Count)
        {

            return null;
        }
        return List[index];
    }
}
