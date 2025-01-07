using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class InfoOfDungeon
{
    public string Name;
    public GameObject Map;
    public float Width = 20.0f;
    public float Height = 10.0f;
}

[Serializable]
public class StatusOfDungeon
{
    public float Time = 90.0f;
    public float Speed = 2.0f;
    public int InitialMoney = 1000;
    public float InitialPopularity = 10.0f;
    public float InitialRiskLevel = 10.0f;

}

[Serializable]
public class DungeonData
{
    public InfoOfDungeon Info;
    public StatusOfDungeon Status;
}

[CreateAssetMenu(menuName = "ScriptableObject/DungeonList", fileName = "DungeonList")]
public class DungeonList : ScriptableObject
{
    public List<DungeonData> List = new List<DungeonData>();

    public DungeonData Get(int index)
    {
        if (index < 0 || index >= List.Count)
        {
            return null;
        }
        return List[index];
    }
}
