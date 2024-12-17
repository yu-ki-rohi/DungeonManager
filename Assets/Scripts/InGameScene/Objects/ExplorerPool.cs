using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerPool : CharacterPool
{
    [SerializeField] private ExplorerList _explorerList;
    private Transform _entrancePosition;
    private Signpost _startDestination;
    private Vector3 _firstDir = Vector3.zero;

    public GameObject Get(int id)
    {
        if (_firstDir == Vector3.zero)
        {
            Debug.LogAssertion("Initialize ExplorerPool !!");
            return null;
        }
        GameObject explorer = Get(_entrancePosition.position);

        // TODO:•ûŒüA–Ú“I’n‚Æ’TõÒî•ñ‚È‚Ç‚Ì‰Šú‰»ˆ—
        if (explorer.TryGetComponent<Explorer>(out var charaBase))
        {
            charaBase.Initialize(_startDestination, _startDestination, _explorerList.GetOptionData(id), _firstDir, this);
        }
        return explorer;
    }

    public void Initialize(Transform entrance, Signpost startSignpost)
    {
        _entrancePosition = entrance;
        _startDestination = startSignpost;
        _firstDir = (_startDestination.transform.position - _entrancePosition.position).normalized;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
