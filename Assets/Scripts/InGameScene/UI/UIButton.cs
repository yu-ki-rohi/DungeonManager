using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    /// <summary>
    /// ボタンを押したときのふるまい
    /// </summary>
    /// <returns>自身を渡す</returns>
    public virtual UIButton PressBehave()
    {
        return this;
    }

    /// <summary>
    /// ボタン上で離された時のふるまい
    /// </summary>
    /// <returns></returns>
    public virtual int ReleaseInBehave()
    {
        return -1;
    }

    /// <summary>
    /// ボタン外で離された時のふるまい
    /// </summary>
    /// <returns></returns>
    public virtual int ReleaseOutBehave()
    {
        return -1;
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
