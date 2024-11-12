using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private Signpost _startSignpost;
    // Start is called before the first frame update
    void Start()
    {
        _startSignpost.SetValue(-1);
        _startSignpost.SetDepth(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
