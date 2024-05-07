using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Alive
{
    public GameObject healthBarPrefab;
    Image healthBar;

    [SerializeField] private ResultsScreen results;
    
    private void OnEnable()
    {
        InitializeHealthBar();
    }

    private void Update()
    {
        if (healthBar) healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, Time.deltaTime * 50);
    }

    void InitializeHealthBar()
    {
        healthBar = Instantiate(healthBarPrefab).transform.GetChild(0).GetChild(2).GetComponent<Image>();
    }

    protected override void Death(KillFlag flag)
    {
        base.Death(flag);
        
        results.ShowResults();
    }
}
