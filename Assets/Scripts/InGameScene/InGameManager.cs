using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private Signpost _startSignpost;
    [SerializeField] private ExplorerPool _explorePool;
    [SerializeField] private float _visitCoolTime = 5.0f; // ‰¼’u‚«

    private float _visitTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _startSignpost.SetValue(-1);
        _startSignpost.SetDepth(0);
    }

    // Update is called once per frame
    void Update()
    {
        if( _visitTimer < 0)
        {
            _explorePool.Get(0);
            _visitTimer = _visitCoolTime;
        }
        else
        {
            _visitTimer -= Time.deltaTime;
        }
    }
}
