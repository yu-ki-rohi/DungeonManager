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

        // TODO:•ûŒüA–Ú“I’n‚Æ’TõÒî•ñ‚Ì‰Šú‰»ˆ—
        if (explorer.TryGetComponent<Explorer>(out var charaBase))
        {
            charaBase.Initialize(_startDestination, _startDestination, _explorerList.GetOptionData(id), _firstDir, this);
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
