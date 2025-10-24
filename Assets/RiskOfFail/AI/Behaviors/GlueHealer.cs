using System;
using System.Collections;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using Pathfinding;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfFail.AI.Behaviors
{
    public class GlueHealer : MonoBehaviour
    {
        public Alive target;

        public float healRadius = 10f;
        public float healFactor = 2;
        public float healCap = 15;
        public AudioClipRandomizer healSound;

        public UnityEvent OnHeal;
        private readonly AIMode currentMode = AIMode.pathing;
        private AIPath ai;
        private Rigidbody2D player;
        private Enemy self;

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
                    var raycastHit = Physics2D.OverlapCircleAll(transform.position, healRadius);
                    var nearby = new List<Alive>();
                    foreach (var e in raycastHit)
                        if (e.TryGetComponent<BossEnemy>(out var boss))
                            nearby.Add(boss);
                        else if (e.TryGetComponent<Enemy>(out var enemy))
                            if (enemy != self && !enemy.TryGetComponent<GlueHealer>(out _))
                                nearby.Add(enemy);

                    if (!target)
                    {
                        if (nearby.Count > 0)
                        {
                            ai.destination = nearby[0].transform.position;
                            // find enemy with lowest health
                            var lowestHealth = nearby[0].health;
                            target = nearby[0];
                            for (var i = 0; i < nearby.Count; i++)
                                if (nearby[i].health < lowestHealth)
                                    target = nearby[i];
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

        private IEnumerator HealLoop()
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
                    target.GiveHealth(Math.Min(target.maxHealth * healFactor, healCap), DamageTypeFlag.Ranged);
            }
        }


        // select enemy to heal

        // go towards

        // throw potion
    }
}