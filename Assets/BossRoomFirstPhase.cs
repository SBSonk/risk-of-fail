using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BossRoomFirstPhase : MonoBehaviour
{
    public BossEnemy boss;

    [Header("Phase End")] public float phaseEndThreshold = 500;
    
    [Header("Damage Phase")] public ShakeData damagePhaseShake;
    public float damagePhaseTimer = 30;
    public float normalDamageReduc = 8;
    public float damagePhaseReduc = 1;
    public bool damagePhase = false;
    public int healthSegments = 4;

    [Header("Attack Timer")] public float attackChoosingTimer = 2f;
    public float minAttackTimer = 2.5f;
    public float maxAttackTimer = 5f;

    public BossAttackV2[] randomAttacks;
    public BossEnemySpawner enemySpawner;

    private BossAttackV2 currentAttackScript;

    private List<BossAttackV2> attackBag;
    private Coroutine attackLoop, currentAttack, damagePhaseTimerCoroutine;
    private float damagePhaseEndHealth, maxDamagePhaseDamage;

    public UnityEvent OnChooseAttack, OnAttack, OnDamagePhaseStart, OnDamagePhaseEnd, OnSpawnEnemy, OnPhaseEnd;

    private bool canAttack = true;
    private void Start()
    {
        ResetAttackBag();

        attackLoop = StartCoroutine(AttackLoop());

        maxDamagePhaseDamage = boss.maxHealth / healthSegments;

        for (int i = 0; i < randomAttacks.Length; i++)
        {
            randomAttacks[i].OnAttackEnd.AddListener(() =>
            {
                print("test");
                StartCoroutine(EnableAttack());
            });
        }
        
        boss.onHit.AddListener(TrackHitDamage);
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(Random.Range(minAttackTimer, maxAttackTimer));
        canAttack = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        canAttack = false;
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            if (canAttack)
            {
                OnChooseAttack?.Invoke();
                BossAttackV2 nextAttack = ChooseAttack();
                // Throw Enemies Half the time
                if (Random.Range(0, 2) == 1)
                {
                    OnSpawnEnemy?.Invoke();
                    StartCoroutine(enemySpawner.Attack());
                }
            
                yield return new WaitForSeconds(attackChoosingTimer);

                OnAttack?.Invoke();
                currentAttack = StartCoroutine(nextAttack.Attack());
                currentAttackScript = nextAttack;

                canAttack = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    void ResetAttackBag()
    {
        attackBag = new List<BossAttackV2>();

        foreach (var attack in randomAttacks)
        {
            attackBag.Add(attack);
        }
    }
    
    public BossAttackV2 ChooseAttack()
    {
        BossAttackV2 attackChosen = attackBag[Random.Range(0, attackBag.Count)];
        attackBag.Remove(attackChosen);
        
        if (attackBag.Count == 0) ResetAttackBag();

        return attackChosen;
    }
    
    public void OnCanisterKill()
    {
        OnSpawnEnemy?.Invoke();
        StartCoroutine(enemySpawner.Attack());
    }
    
    public void StartDamagePhase()
    {
        OnDamagePhaseStart?.Invoke();
        CameraShakerHandler.Shake(damagePhaseShake);
        
        damagePhase = true;
        boss.damageReduction = damagePhaseReduc;
        
        damagePhaseEndHealth = boss.health - maxDamagePhaseDamage;

        damagePhaseTimerCoroutine = StartCoroutine(DamagePhaseTimer());
        StopCoroutine(currentAttack);
        currentAttackScript.CancelAttack();
        StopCoroutine(attackLoop);
    }

    void TrackHitDamage(float _)
    {
        // Cancel if reached threshhold
        if (damagePhase)
        {
            if (boss.health <= damagePhaseEndHealth) EndDamagePhase();
        }

        if (boss.health <= phaseEndThreshold) OnPhaseEnd?.Invoke();
    }
    
    IEnumerator DamagePhaseTimer()
    {
        yield return new WaitForSeconds(damagePhaseTimer);

        EndDamagePhase();
    }

    void EndDamagePhase()
    {
        OnDamagePhaseEnd?.Invoke();
        
        if (damagePhaseTimerCoroutine != null) StopCoroutine(damagePhaseTimerCoroutine);
        
        damagePhase = false;
        boss.damageReduction = normalDamageReduc;
        attackLoop = StartCoroutine(AttackLoop());
    }
}
