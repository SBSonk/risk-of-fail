using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleManager : MonoBehaviour
{
    public static StyleManager instance;

    public float maxStyle = 100;
    public float style;

    public float baseStyleDecrease = 0.1f;
    public float idleStyleDecrease = 1;
    
    public bool inBattle;
    
    public Image styleBar;
    
    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        DecreaseStyle();

        ClampStyle();        
        
        UpdateStyleBar();
    }

    void DecreaseStyle()
    {
        float decRate = baseStyleDecrease;

        if (!inBattle) decRate = idleStyleDecrease;
        
        style -= decRate * Time.deltaTime;
    }

    void ClampStyle()
    {
        style = Mathf.Clamp(style, 0, maxStyle);
    }

    void UpdateStyleBar()
    {
        styleBar.fillAmount = style / maxStyle;
    }
}
