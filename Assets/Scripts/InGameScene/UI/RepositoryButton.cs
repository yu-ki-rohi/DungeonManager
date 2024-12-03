using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryButton : UIButton
{
    [SerializeField] protected int _index;

    public override UIButton PressBehave()
    {
        return base.PressBehave();
    }

    public override int ReleaseInBehave()
    {
        return _index;
    }

    public override int ReleaseOutBehave()
    {
        return _index;
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
