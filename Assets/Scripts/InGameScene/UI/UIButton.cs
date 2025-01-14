using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public enum  UIType
    {
        Default,
        MonsterRepository,
        ItemRepository
    }
    /// <summary>
    /// ボタンを押したときのふるまい
    /// </summary>
    /// <returns>自身を渡す</returns>
    public virtual UIButton PressBehave(out UIType uiType, out int index)
    {
        index = 0;
        uiType = UIType.Default;
        return this;
    }

    /// <summary>
    /// ボタン上で離された時のふるまい
    /// </summary>
    /// <returns></returns>
    public virtual UIType ReleaseInBehave(out int index)
    {
        index = 0;
        return UIType.Default;
    }

    /// <summary>
    /// ボタン外で離された時のふるまい
    /// </summary>
    /// <returns></returns>
    public virtual UIType ReleaseOutBehave(out int index)
    {
        index = 0;
        return UIType.Default;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
