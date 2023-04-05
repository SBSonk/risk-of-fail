using System;
using System.Collections;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;


public class GlueHealer : MonoBehaviour
{
    Rigidbody2D player;
    Enemy self;
    private AIPath ai;
    private AIMode currentMode = AIMode.pathing;
    public Enemy target;

    public float healRadius = 10f;
    public float healFactor = 2;
    public float healCap = 15;
    public AudioClipRandomizer healSound;

    public UnityEvent OnHeal;

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
                        if (enemy != self && !enemy.TryGetComponent<GlueHealer>(out _)) nearby.Add(enemy);
                    }
                }

                if (!target)
                {
                    if (nearby.Count > 0)
                    {
                        ai.destination = nearby[0].transform.position;
                        // find enemy with lowest health
                        float lowestHealth = nearby[0].health;
                        target = nearby[0];
                        for (int i = 0; i < nearby.Count; i++)
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
                }
                break;
        }
    }

    IEnumerator HealLoop()
    {
        while (true)
        {
            if (target && target.health < target.maxHealth && ai.reachedDestination)
            {
                OnHeal?.Invoke();
                healSound.PlaySFX();
            }

            yield return new WaitForSeconds(.5f);
            
            if (target && target.health < target.maxHealth && ai.reachedDestination)
            {
                target.GiveHealth(Math.Max(target.maxHealth * healFactor, healCap));
            }
        }
    }


    // select enemy to heal

    // go towards

    // throw potion
}
