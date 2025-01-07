using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerPool : CharacterPool
{
    [SerializeField] private ExplorerList _explorerList;
    [SerializeField] private InGameManager _gameManager;
    private Transform _entrancePosition;
    private Signpost _startDestination;
    private Vector3 _firstDir = Vector3.zero;

    public GameObject Get(int id)
    {
        if (_firstDir == Vector3.zero)
        {
            // 初期化不良
            Debug.LogAssertion("Initialize ExplorerPool !!");
            return null;
        }
        if(id < 0 || id >= _explorerList.List.Count)
        {
            // アクセス違反
            Debug.LogAssertion("Illegal Access in Explorer List !!");
            return null;
        }

        GameObject explorer = Get(_entrancePosition.position);

        // TODO:方向、目的地と探索者情報などの初期化処理
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

    public override void Release(GameObject obj, int index = 0, int[] intData = null, float[] floatData = null)
    {
        switch(index)
        {
            case 0:

                break;
            case 1:
                if(intData != null && intData.Length > 0)
                {
                    _gameManager.EarnMoney(intData[0]);
                }
                break;
        }
        base.Release(obj, index, intData, floatData);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
