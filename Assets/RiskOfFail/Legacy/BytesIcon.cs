using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BytesIcon : MonoBehaviour
{
    public float strength = 2, duration = .5f;
    public int clicksNeeded = 10;
    private int clicks;

    public Image background, logo;

    public UnityEvent OnReachedCount;

    private bool active = true;
    
    public void Click()
    {
        transform.DOShakePosition(duration, strength).SetEase(Ease.OutFlash);
        transform.DOShakeRotation(duration, strength).SetEase(Ease.OutFlash);

        if (!active) return;
        
        clicks++;

        if (clicks > 10)
        {
            background.DOFade(0, duration);
            logo.DOFade(0, duration);
            
            OnReachedCount?.Invoke();
            active = false;
        }
    }
}
