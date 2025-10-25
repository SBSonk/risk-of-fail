using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BossRoomSecondPhase : MonoBehaviour
{
    public BossEnemy boss;
    public SpawnHealCanisters healSpawner;

    [Header("Damage Phase")] public ShakeData damagePhaseShake;
    public float damagePhaseTimer = 30;
    public float normalDamageReduc = 8;
    public float damagePhaseReduc = 1;
    public bool damagePhase;
    public int healthSegments = 4;

    [Header("Attack Timer")] public float attackChoosingTimer = 2f;
    public float minAttackTimer = 2.5f;
    public float maxAttackTimer = 5f;
    public int minAttacksBeforeSpecial = 2;
    public int maxAttacksBeforeSpecial = 3;

    public BossAttackV2[] heavyAttack;
    public BossAttackV2[] randomAttacks;
    public BossEnemySpawner enemySpawner;

    public UnityEvent OnChooseAttack, OnAttack, OnDamagePhaseStart, OnDamagePhaseEnd, OnSpawnEnemy;

    private List<BossAttackV2> attackBag;
    private Coroutine attackLoop, currentAttack, damagePhaseTimerCoroutine;
    private int attacksBeforeSpecial = 0;

    private bool canAttack = true;

    private BossAttackV2 currentAttackScript;
    private float damagePhaseEndHealth, maxDamagePhaseDamage;

    private void Start()
    {
        ResetAttackBag();

        attackLoop = StartCoroutine(AttackLoop());

        maxDamagePhaseDamage = boss.maxHealth / healthSegments;

        for (var i = 0; i < randomAttacks.Length; i++)
            randomAttacks[i].OnAttackEnd.AddListener(() => { StartCoroutine(EnableAttack()); });

        //boss.onHit.AddListener(TrackHitDamage);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (currentAttack != null) currentAttackScript.CancelAttack();
        canAttack = false;
    }

    private IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(Random.Range(minAttackTimer, maxAttackTimer));
        canAttack = true;
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (canAttack)
            {
                OnChooseAttack?.Invoke();
                var nextAttack = ChooseAttack();
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

    private void ResetAttackBag()
    {
        attackBag = new List<BossAttackV2>();

        foreach (var attack in randomAttacks) attackBag.Add(attack);
    }

    public BossAttackV2 ChooseAttack()
    {
        var attackChosen = attackBag[Random.Range(0, attackBag.Count)];
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

    private void TrackHitDamage(float _, DamageTypeFlag __) 
    {
        // Cancel if reached threshhold
        if (damagePhase)
            if (boss.health <= damagePhaseEndHealth)
                EndDamagePhase();
    }

    private IEnumerator DamagePhaseTimer()
    {
        yield return new WaitForSeconds(damagePhaseTimer);

        EndDamagePhase();
    }

    private void EndDamagePhase()
    {
        OnDamagePhaseEnd?.Invoke();

        if (damagePhaseTimerCoroutine != null) StopCoroutine(damagePhaseTimerCoroutine);

        damagePhase = false;
        boss.damageReduction = normalDamageReduc;
        attackLoop = StartCoroutine(AttackLoop());
    }
}