using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager main;
    [SerializeField] private DialogueFX dialogueFx;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshPro tmp;
    Color defaultPanelColor, defaultTextColor;

    public UnityEvent OnDialogueTrigger;
    public UnityEvent OnDialogueFinish;
    
    private void Awake()
    {
        main = this;
        defaultTextColor = tmp.color;
    }

    public void ShowText(string text)
    {
        StopAllCoroutines();
        
        OnDialogueTrigger?.Invoke();
        StartCoroutine(PanelAnimation(text));
    }
    
    public void ShowText(DialogueText[] dialogue, UnityEvent callback)
    {
        StopAllCoroutines();
        
        OnDialogueTrigger?.Invoke();
        StartCoroutine(PanelAnimation(dialogue, callback));
    }

    IEnumerator PanelAnimation(string text, UnityEvent callback = null, bool append = false, float disappearTime = 0.5f)
    {
        anim.Play("DialogueOpen");
        tmp.text = "";

        yield return new WaitForSeconds(0.5f);
        
        bool finishedDisplaying = false;
        
        tmp.color = defaultTextColor;
        dialogueFx.PlayText(text, append, () => { finishedDisplaying = true;});

        while (!finishedDisplaying) yield return null;

        yield return new WaitForSeconds(disappearTime);

        callback?.Invoke();
        
        HideText();
    }
    
    IEnumerator PanelAnimation(DialogueText[] dialogue, UnityEvent callback = null)
    {
        anim.Play("DialogueOpen");
        tmp.text = "";

        yield return new WaitForSeconds(0.5f);

        foreach (var d in dialogue)
        {
            bool finishedDisplaying = false;
            
            tmp.color = defaultTextColor;
            dialogueFx.PlayText(d.text, d.append, () => { finishedDisplaying = true;});

            while (!finishedDisplaying) yield return null;

            yield return new WaitForSeconds(d.holdTime);
        }
        
        callback?.Invoke();
        
        HideText();
    }
    
    public void HideText()
    {
        StartCoroutine(SprFunctions.Fade(tmp, tmp.color, Color.clear, 0.25f));

        OnDialogueFinish?.Invoke();
        anim.Play("DialogueClose");
    }
}
