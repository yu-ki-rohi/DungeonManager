using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [SerializeField] private Sprite _closedBox;
    [SerializeField] private Sprite _openBox;
    private SpriteRenderer _renderer;
    private int _value = 0;

    public bool IsClosed()
    {
        return _value > 0;
    }

    public void SetTreasure(int value)
    {
        _value = value;
        _renderer.sprite = _closedBox;
    }

    public int GetTreasure()
    {
        int value = _value;
        _value = 0;
        _renderer.sprite = _openBox;
        return value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = _openBox;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
