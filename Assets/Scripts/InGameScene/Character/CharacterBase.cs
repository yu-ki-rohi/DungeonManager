using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private Signpost _destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_destination != null)
        {
            if ((transform.position - _destination.transform.position).sqrMagnitude < 0.01f)
            {
                _destination = _destination.GetNextDestination();
            }
        }

        transform.position += (_destination.transform.position - transform.position).normalized * Time.deltaTime;
    }
}
