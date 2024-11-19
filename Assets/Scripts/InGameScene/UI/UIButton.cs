using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public virtual UIButton PressBehave()
    {
        return this;
    }

    public virtual int ReleaseBehave()
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
