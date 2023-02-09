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
    [SerializeField] Image panel;
    [SerializeField] private TextMeshProUGUI tmp;
    Color defaultPanelColor, defaultTextColor;

    private float hideTime = 0.5f;
    
    private void Awake()
    {
        main = this;
        defaultPanelColor = panel.color;
        defaultTextColor = tmp.color;
        
        panel.color = Color.clear;
    }

    public void ShowText(string text, float disappearTime = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(panel, Color.clear, defaultPanelColor, 0.5f));

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
        StartCoroutine(SprFunctions.Fade(panel, panel.color, Color.clear, 1f));
        StartCoroutine(SprFunctions.Fade(tmp, tmp.color, Color.clear, 0.25f));
    }
}
