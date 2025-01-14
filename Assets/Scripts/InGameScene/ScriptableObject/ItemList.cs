using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class InfoOfItem
{
    public string Name;
    public Sprite Sprite;
}

[Serializable]
public class StatusOfItem
{
    public int Value = 100;
}

[Serializable]
public class ItemData
{
    public InfoOfItem Info;
    public StatusOfItem Status;
}

[CreateAssetMenu(menuName = "ScriptableObject/ItemList", fileName = "ItemList")]
public class ItemList : ScriptableObject
{
    public List<ItemData> List = new List<ItemData>();

    public ItemData Get(int index)
    {
        if (index < 0 || index >= List.Count)
        {
            return null;
        }
        return List[index];
    }
}
