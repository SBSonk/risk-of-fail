using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Alive
{
    public GameObject healthBarPrefab;
    Image healthBar;

    private void Start()
    {
        InitializeHealthBar();
    }

    private void Update()
    {
        if (healthBar) healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, Time.deltaTime * 50);
    }

    void InitializeHealthBar()
    {
        healthBar = Instantiate(healthBarPrefab).transform.Find("Health").GetComponent<Image>();
    }
}
