using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private float timerSeconds = 5;
    public UnityEvent OnTimerStart, OnTimerEnd;

    private void Awake()
    {
        if (playOnAwake) StartTimer();
    }

    public void StartTimer()
    {
        Invoke("TimerEnd", timerSeconds);
        OnTimerStart?.Invoke();
    }

    void TimerEnd()
    {
        OnTimerEnd?.Invoke();
    }
}
