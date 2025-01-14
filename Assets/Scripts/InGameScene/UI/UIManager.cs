using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _popularityText;
    [SerializeField] private TextMeshProUGUI _riskLevelText;



    public void ReflectMoney(int money)
    {
        _moneyText.text = money.ToString();
    }

    public void ReflectTime(float time)
    {
        int minute = (int)(time / 60.0f);
        int seconds = (int)(time - minute * 60.0f);

        _timerText.text = minute.ToString("00") + ":" + seconds.ToString("00");
    }

    public void ReflectPopularity(float popularity)
    {
        _popularityText.text = popularity.ToString();
    }

    public void ReflectRiskLevel(float riskLevel)
    {
        _riskLevelText.text = riskLevel.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
