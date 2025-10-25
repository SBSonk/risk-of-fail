using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager main;
    [SerializeField] private DialogueFX dialogueFx;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshPro tmp;

    public UnityEvent OnDialogueTrigger;
    public UnityEvent OnDialogueFinish;
    private Color defaultPanelColor, defaultTextColor;

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

    private IEnumerator PanelAnimation(string text, UnityEvent callback = null, bool append = false,
        float disappearTime = 0.5f)
    {
        anim.Play("DialogueOpen");
        tmp.text = "";

        yield return new WaitForSeconds(0.5f);

        var finishedDisplaying = false;

        tmp.color = defaultTextColor;
        dialogueFx.PlayText(text, append, () => { finishedDisplaying = true; });

        while (!finishedDisplaying) yield return null;

        yield return new WaitForSeconds(disappearTime);

        callback?.Invoke();

        HideText();
    }

    private IEnumerator PanelAnimation(DialogueText[] dialogue, UnityEvent callback = null)
    {
        anim.Play("DialogueOpen");
        tmp.text = "";

        yield return new WaitForSeconds(0.5f);

        foreach (var d in dialogue)
        {
            var finishedDisplaying = false;

            tmp.color = defaultTextColor;
            dialogueFx.PlayText(d.text, d.append, () => { finishedDisplaying = true; });

            while (!finishedDisplaying) yield return null;

            yield return new WaitForSeconds(d.holdTime);
        }

        callback?.Invoke();

        HideText();
    }

    public void HideText()
    {
        StartCoroutine(HelperFunctions.Fade(tmp, tmp.color, Color.clear, 0.25f));

        OnDialogueFinish?.Invoke();
        anim.Play("DialogueClose");
    }
}