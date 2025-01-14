using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryButton : UIButton
{
    [SerializeField] protected int _index;
    [SerializeField] protected UIType _type;

    public override UIButton PressBehave(out UIType uiType, out int index)
    {
        uiType = _type;
        index = _index;
        return this;
    }

    public override UIType ReleaseInBehave(out int index)
    {
        index = _index;
        return _type;
    }

    public override UIType ReleaseOutBehave(out int index)
    {
        index = _index;
        return _type;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
