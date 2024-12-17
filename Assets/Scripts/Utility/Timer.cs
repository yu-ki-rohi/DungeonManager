using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TimeoutBehavior();

public class Timer
{
    public float Time;
    private TimeoutBehavior _behavior;
    private float _timer;

    public Timer(TimeoutBehavior behavior, float time)
    {
        _behavior = behavior;
        _timer = 0.0f;
        Time = time;
    }

    public void Update(float deltaTime)
    {
        if(_timer > Time)
        {
            _behavior();
            _timer = 0.0f;
        }
        else
        {
            _timer += deltaTime;
        }
    }
}
