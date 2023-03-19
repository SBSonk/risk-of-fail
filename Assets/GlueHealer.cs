using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GlueHealer : MonoBehaviour
{
    Rigidbody2D player;
    Enemy self;
    private AIPath ai;
    private AIMode currentMode = AIMode.pathing;
    public Enemy target;

    public float healRadius = 10f;
    public float healFactor = 2;

    private void Awake()
    {
        ai = GetComponent<AIPath>();
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(HealLoop());
    }

    private void Update()
    {
        if (!PlayerStatus.IsAlive) return;
        
        switch (currentMode)
        {
            case AIMode.pathing:
                // Go to random position near player
                Collider2D[] raycastHit = Physics2D.OverlapCircleAll(transform.position, healRadius);
                List<Enemy> nearby = new List<Enemy>();
                foreach (var e in raycastHit)
                {
                    if (e.TryGetComponent<Enemy>(out var enemy))
                    {
                        nearby.Add(enemy);
                    }
                }

                if (!target)
                {
                    if (nearby.Count > 1)
                    {
                        ai.destination = nearby[1].transform.position;
                        // find enemy with lowest health
                        float lowestHealth = nearby[1].health;

                        for (int i = 1; i < nearby.Count; i++)
                        {
                            if (nearby[i].health < lowestHealth) target = nearby[i];
                        }
                    }
                    else
                    {
                        ai.destination = player.position;
                    }
                }
                else
                {
                    ai.destination = target.transform.position;

                    // Finish healing
                    if (Mathf.RoundToInt(target.health) == Mathf.RoundToInt(target.maxHealth))
                    {
                        target = null;
                    }
                }
                break;
        }
    }

    IEnumerator HealLoop()
    {
        while (true)
        {
            if (target && target.health < target.maxHealth)
            {
                target.GiveHealth(healFactor);
            }

            yield return new WaitForSeconds(.25f);
        }
    }


    // select enemy to heal

    // go towards

    // throw potion
}
