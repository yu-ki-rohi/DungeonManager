using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private DungeonList _dungeonList;
    [SerializeField] private ExplorerPool _explorePool;
    [SerializeField] private MonsterPool _monsterPool;
    [SerializeField] private float _visitCoolTime = 5.0f; // ‰¼’u‚«
    private int _dungeonID = 0;
    private Timer _visitTimer;
    private int _money = 10;

    private void ExplorerVisit()
    {
        _explorePool.Get(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject map = Instantiate(_dungeonList.Get(_dungeonID).Info.Map);
        if(map.TryGetComponent<Dungeon>(out var dungeon))
        {
            dungeon.StartSignpost.SetValue(-1);
            dungeon.StartSignpost.SetDepth(0);

            _explorePool.Initialize(dungeon.Entrance, dungeon.StartSignpost);

            _monsterPool.Initialize(dungeon.SignpostsParent);
        }

        _visitTimer = new Timer(ExplorerVisit, _visitCoolTime);
        ExplorerVisit();
    }

    // Update is called once per frame
    void Update()
    {
        _visitTimer.Update(Time.deltaTime);
    }
}
