using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelFade : MonoBehaviour
{
    private static LevelFade instance;

    private Animator anim;

    private Action finishCallback;
    private Image img;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void AnimationFinished()
    {
        finishCallback?.Invoke();
    }

    public static void FadeOut(Action callback = null)
    {
        instance.finishCallback = callback;
        instance.anim.Play("FadeOut");
    }

    public static void FadeIn(Action callback = null)
    {
        instance.finishCallback = callback;
        instance.anim.Play("FadeIn");
    }
}