using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private DungeonList _dungeonList;
    [SerializeField] private ExplorerPool _explorePool;
    [SerializeField] private MonsterPool _monsterPool;
    [SerializeField] private float _visitCoolTime = 5.0f; // 仮置き
    [SerializeField] private float _cameraSpeed = 5.0f; // 仮置き
    [SerializeField] private UIManager _uiManager;
    private int _dungeonID = 0;
    private Timer _visitTimer;
    private Timer _gameTimer;
    private int _money = 10;
    private float _popularity = 0.0f;
    private float _riskLevel = 1.0f;


   

    public bool CanPurchase(int cost)
    {
        if (cost > _money)
        {
            return false;
        }

        _money -= cost;
        _uiManager.ReflectMoney(_money);
        return true;
    }

    public void EarnMoney(int money)
    {
        _money += money;
        _uiManager.ReflectMoney(_money);
    }

    public void UpdatePopularity(float popularity)
    {
        _popularity += popularity;
        _uiManager.ReflectPopularity(_popularity);
    }

    public void UpdateRiskLevel(float risk)
    {
        _riskLevel += risk;
        _uiManager.ReflectRiskLevel(_riskLevel);
    }
    private void ExplorerVisit()
    {
        _explorePool.Get(0, _riskLevel);
    }

    private void GameClear()
    {

    }

    private void AdjustCamera()
    {

        if (Mathf.Abs(Camera.main.transform.position.x) > _dungeonList.Get(_dungeonID).Info.Width)
        {
            if (Camera.main.transform.position.x > 0)
            {
                Camera.main.transform.position += Vector3.left * Time.deltaTime * _cameraSpeed;
            }
            else
            {
                Camera.main.transform.position += Vector3.right * Time.deltaTime * _cameraSpeed;
            }
        }

        if (Mathf.Abs(Camera.main.transform.position.y) > _dungeonList.Get(_dungeonID).Info.Height)
        {
            if (Camera.main.transform.position.y > 0)
            {
                Camera.main.transform.position += Vector3.down * Time.deltaTime * _cameraSpeed;
            }
            else
            {
                Camera.main.transform.position += Vector3.up * Time.deltaTime * _cameraSpeed;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _money = _dungeonList.Get(_dungeonID).Status.InitialMoney;
        _popularity = _dungeonList.Get(_dungeonID).Status.InitialPopularity;
        _riskLevel = _dungeonList.Get(_dungeonID).Status.InitialRiskLevel;
        GameObject map = Instantiate(_dungeonList.Get(_dungeonID).Info.Map);
        if(map.TryGetComponent<Dungeon>(out var dungeon))
        {
            dungeon.StartSignpost.SetValue(-1);
            dungeon.StartSignpost.SetDepth(0);

            _explorePool.Initialize(dungeon.Entrance, dungeon.StartSignpost);
            _monsterPool.Initialize(dungeon.SignpostsParent);
            

            Camera.main.transform.position = new Vector3(
                dungeon.StartSignpost.gameObject.transform.position.x,
                dungeon.StartSignpost.gameObject.transform.position.y,
                -10);
        }

        _uiManager.ReflectMoney(_money);

        _gameTimer = new Timer(GameClear, _dungeonList.Get(_dungeonID).Status.Time);
        _gameTimer.PrepareCountDown();
        _uiManager.ReflectTime(_gameTimer.CurrentTime);

        _uiManager.ReflectRiskLevel(_riskLevel);
        _uiManager.ReflectPopularity(_popularity);

        _visitTimer = new Timer(ExplorerVisit, _visitCoolTime);
        ExplorerVisit();
    }

    // Update is called once per frame
    void Update()
    {
        _visitTimer.CountUp(Time.deltaTime);

        _gameTimer.CountDown(Time.deltaTime);
        _uiManager.ReflectTime(_gameTimer.CurrentTime);

        AdjustCamera();
    }
}
