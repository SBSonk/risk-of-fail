using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEntranceCutscene : MonoBehaviour
{
    public UnityEvent OnCutsceneStart, OnCutsceneEnd;

    private void Start()
    {
        OnCutsceneStart?.Invoke();
    }

    public void EndCutscene()
    {
        OnCutsceneEnd?.Invoke();
        Destroy(gameObject);
    }
}
