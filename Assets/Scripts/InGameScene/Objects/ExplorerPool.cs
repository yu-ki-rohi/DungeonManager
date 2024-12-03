using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerPool : CharacterPool
{
    [SerializeField] private Transform _entrancePosition;
    [SerializeField] private Signpost _startDestination;
    [SerializeField] private ExplorerList _explorerList;
    private Vector3 _firstDir;

    public GameObject Get(int id)
    {
        GameObject explorer = Get(_entrancePosition.position);

        // TODO:方向、目的地とモンスター情報の初期化処理
        if (explorer.TryGetComponent<CharacterBase>(out var charaBase))
        {
            charaBase.Initialize(_startDestination, null, _explorerList.Get(id), _firstDir, this);
        }
        return explorer;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _firstDir = (_startDestination.transform.position - _entrancePosition.position).normalized;
    }
}
