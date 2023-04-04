using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager main;
    [SerializeField] private DialogueFX dialogueFx;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshPro tmp;
    Color defaultPanelColor, defaultTextColor;
    
    private float hideTime = 0.5f;
    
    private void Awake()
    {
        main = this;
        defaultTextColor = tmp.color;
    }

    public void ShowText(string text, float disappearTime = 0.5f)
    {
        StopAllCoroutines();

        StartCoroutine(PanelAnimation(text, disappearTime));
    }

    IEnumerator PanelAnimation(string text, float disappearTime = 0.5f)
    {
        anim.Play("DialogueOpen");
        tmp.text = "";

        yield return new WaitForSeconds(0.5f);
        
        // TODO: calculate size based on text length
        
        tmp.text = text;
        tmp.color = defaultTextColor;
        dialogueFx.PlayText();
        hideTime = disappearTime;
    }
    
    public void HideText()
    {
        StartCoroutine(HideTextDelay());
    }

    IEnumerator HideTextDelay()
    {
        yield return new WaitForSeconds(hideTime);
        StartCoroutine(SprFunctions.Fade(tmp, tmp.color, Color.clear, 0.25f));
        
        anim.Play("DialogueClose");
    }
}
