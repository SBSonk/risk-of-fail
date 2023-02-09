using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea]
    [SerializeField] string text;

    [SerializeField] private float hideDelay = 3;

    [SerializeField] private bool active = true;
    [SerializeField] private bool disableOnTrigger = true;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || !active) return;
        
        DialogueManager.main.ShowText(text, hideDelay);
        
        if (disableOnTrigger) SetActive(false);
    }

    public void SetActive(bool val) => active = val;
}
