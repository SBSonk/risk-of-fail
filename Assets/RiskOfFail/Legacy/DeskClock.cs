using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DeskClock : MonoBehaviour
{
    public bool startOnAwake;
    public float time = 10;

    public TextMeshPro text;
    public AudioSource startSound, tickingSound;
    private bool ticking = false;

    public UnityEvent OnTimerStart, OnTimerEnd;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
        if (startOnAwake) StartClock();
    }

    private void Update()
    {
        if (!ticking) return;
        
        TickClock();
        UpdateText();
    }

    void TickClock()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            ticking = false;
            
            anim.Play("ClockFlicker");
            StartCoroutine(StopFlicker());
            OnTimerEnd?.Invoke();
        }
    }

    void UpdateText()
    {
        if (time <= 0) text.SetText("00.00");
        else text.SetText(time.ToString("00.00"));
    }

    public void StartClock()
    {
        ticking = true;

        StartCoroutine(StartSound());
        OnTimerStart?.Invoke();
    }

    IEnumerator StartSound()
    {
        startSound.Play();

        yield return new WaitForSeconds(.25f);

        tickingSound.Play();
    }

    IEnumerator StopFlicker()
    {
        yield return new WaitForSeconds(5.25f);
        
        anim.Play("ClockFlicker", 0, 1);
        anim.speed = 0;
    }
}
