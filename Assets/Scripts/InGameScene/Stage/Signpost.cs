using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour
{
    [SerializeField] private Signpost[] _adjacentSignposts;
    [SerializeField] private bool _isRoom = false;

    // Editor表示用


    // 公開メソッド
    public Signpost GetNextDestination()
    {
        if (_adjacentSignposts.Length <= 0)
        {
            return this;
        }

        int judge = Random.Range(0, _adjacentSignposts.Length);
        return _adjacentSignposts[judge];
    }


    // 非公開メソッド


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Editor表示用
    private void OnDrawGizmos()
    {
        for(int i = 0; i < _adjacentSignposts.Length; i++)
        {
            if (_adjacentSignposts[i] != null)
            {
                Gizmos.DrawLine(transform.position, _adjacentSignposts[i].transform.position);

                // 矢じりの描画
                float ratio = 0.3f;
                Vector3 vec = _adjacentSignposts[i].transform.position - transform.position;
                float scale = 0.5f;
                Gizmos.DrawLine(
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position,
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position +
                    (Vector3.Cross(vec, Vector3.forward) - vec).normalized * scale);
                Gizmos.DrawLine(
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position,
                    ratio * transform.position + (1.0f - ratio) * _adjacentSignposts[i].transform.position -
                    (Vector3.Cross(vec, Vector3.forward) + vec).normalized * scale);
            }
        }
    }
}
