using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class EraserSlamV2 : MonoBehaviour
{
    [Header("Attack")] public float damageAmount = 15;
    public float stunLength = 1;
    public KillFlag killFlag = KillFlag.AreaOfEffect;
    public float startYOffset = 10;
    public float fallLength = 5;
    
    [Header("Visuals")]
    public Vector3 hitOffset;
    public Vector3 hitSize;
    public SpriteRenderer eraserSprite, shadowSprite;
    public Color endShadowColor = Color.black;
    public float disappearTime = 2;
    public float fadeTime = 3;
    public float appearTime = .5f;
    
    public LayerMask playerLayer;

    public UnityEvent OnFallBegin, OnSlam;

    private void Start()
    {
        StartCoroutine(Slam());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + hitOffset, hitSize);
    }

    private IEnumerator Slam()
    {
        Color whiteTrans = new Color(1, 1, 1, 0);
        
        OnFallBegin?.Invoke();
        
        eraserSprite.color = whiteTrans;
        eraserSprite.DOColor(Color.white, appearTime);

        eraserSprite.transform.localPosition = Vector3.up * startYOffset;
        eraserSprite.transform.DOMoveY(transform.position.y, fallLength).SetEase(Ease.InBack);
        shadowSprite.DOColor(endShadowColor, fallLength).SetEase(Ease.InBack);
        
        eraserSprite.sortingOrder = 10;
        
        yield return new WaitForSeconds(fallLength);

        eraserSprite.sortingOrder = 0;
        
        OnSlam?.Invoke();
        CheckHit();

        yield return new WaitForSeconds(disappearTime);

        
        eraserSprite.DOColor(whiteTrans, fadeTime);
        shadowSprite.DOColor(whiteTrans, fadeTime);
    }

    void CheckHit()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + hitOffset, hitSize, 0, playerLayer);

        foreach (var col in hit)
        {
            print(col.name);
            if (col.TryGetComponent(out PlayerStatus p))
            {
                p.GiveDamage(damageAmount, stunLength, killFlag);
                
                return;
            }
        }
    }
}
