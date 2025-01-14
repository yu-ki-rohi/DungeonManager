using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StatusOfExplorer
{
    public float Stamina = 60.0f;
    public int ExpectedValue = 50;
    public int Money = 500;
}

[Serializable]
public class ExplorerData : CharaData
{
    public StatusOfExplorer OptionStatus;
}

[CreateAssetMenu(menuName = "ScriptableObject/CharacterList/Explorer", fileName = "ExplorerList")]
public class ExplorerList : CharacterList
{
    public List<ExplorerData> List = new List<ExplorerData>();

    public override CharaData Get(int index)
    {
        if (index < 0 || index >= List.Count)
        {

            return null;
        }
        return List[index];
    }

    public ExplorerData GetOptionData(int index)
    {
        if (index < 0 || index >= List.Count)
        {

            return null;
        }
        return List[index];
    }
}
