using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StatusOfExplorer
{
    public float StaminaMax = 60.0f;
    public float ExpectedValue = 1.0f;
    public int Money = 10;
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
