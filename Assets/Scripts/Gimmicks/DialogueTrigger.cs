using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueText[] dialogue;

    [SerializeField] private float reactivateDelay = 6;
    [SerializeField] private float hideDelay = 3;

    [SerializeField] private bool active = true;
    [SerializeField] private bool disableOnTrigger = true;

    public UnityEvent OnDialogueTrigger;
    public UnityEvent OnDialogueFinish;

    private void Start()
    {
        DialogueManager.main.OnDialogueTrigger.AddListener(() => this.OnDialogueTrigger?.Invoke());
        DialogueManager.main.OnDialogueFinish.AddListener(() => this.OnDialogueFinish?.Invoke());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || !active) return;
        
        DialogueManager.main.ShowText(dialogue);
        
        if (disableOnTrigger) SetActive(false);
        else
        {
            active = false;
            Invoke(nameof(SetActive), reactivateDelay);
        }
    }

    public void SetActive(bool val) => active = val;

    public void SetActive() => active = true;
}

[System.Serializable]
public struct DialogueText
{
    [TextArea] public string text;
    public float holdTime;
    public bool append;
}