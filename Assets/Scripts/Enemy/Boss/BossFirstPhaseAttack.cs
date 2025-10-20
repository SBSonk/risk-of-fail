    using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossFirstPhaseAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private Spawning2 roomSpawner;
    [SerializeField] private BossAttack spawner;
    [SerializeField] private BossAttack closeRangeAttack;
    [SerializeField] private BossAttack[] specialAttacks;
    [SerializeField] private BossAttack[] randomAttacks;
    [SerializeField] private int minAmountForSpecial = 3;
    [SerializeField] private int maxAmountForSpecial = 5;
    [SerializeField] private Collider2D col;

    [SerializeField] private BossState currentState = BossState.Idle;
    
    [Header("Idle")] 
    [SerializeField] private float idleBufferTime = 3;

    [Header("ChoosingAttack")] 
    [SerializeField] private float chooseBufferTime = 1;
    private BossAttack nextAttack;

    [Header("Attacking")] private float nextAttackTime;
    [SerializeField] private float postAttackBuffer = 1;

    [Header("PreDamagePhase")] [SerializeField]
    private float preDamagePhaseLength = 10;
    
    [Header("DamagePhase")] [SerializeField]
    private BossEnemy self;

    [SerializeField] private float damagePhaseHealth = 250;
    private float damagePhaseStartHealth;

    [SerializeField] private ShakeData damagePhaseStartShake;
    [SerializeField] private ShakeData damagePhaseEndShake;
    [SerializeField] private BossCircleHandler bossCirclePrefab;
    [SerializeField] private int attacksBeforePreDamagePhase = 3;
    [SerializeField] private float damagePhaseLength = 7.5f;
    [SerializeField] private float postDamagePhaseCooldown = 5;
    public int attacksDone;
    [SerializeField] private int initialBossCircles = 3;
    private bool damagePhase;
    
    private float timer;
    private int attacksBeforeSpecial;

    private BossCircleHandler handler;

    [SerializeField] private BossDropSpawner[] itemSpawners;

    private void Start()
    {
        attacksBeforeSpecial = Random.Range(minAmountForSpecial, maxAmountForSpecial);
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                // Taunts player or something

                if (timer >= idleBufferTime) SwitchState(BossState.ChoosingAttack);
                break;
            
            case BossState.Attacking:
                if (timer >= nextAttackTime)
                {
                    if (attacksDone >= attacksBeforePreDamagePhase)
                    {
                        EnterPreDamagePhase();
                    }
                    else
                    {
                        SwitchState(BossState.AttackCooldown);
                        nextAttackTime = postAttackBuffer;
                    }
                }
                break;
            
            case BossState.AttackCooldown:
                if (timer >= nextAttackTime) SwitchState(BossState.Idle);
                break;
            
            case BossState.DamagePhase:
                if (self.health <= damagePhaseStartHealth - damagePhaseHealth)
                {
                    EndDamagePhase();

                    CameraShakerHandler.Shake(damagePhaseEndShake);
                    
                    print("end");
                }
                break;
        }

        timer += Time.deltaTime;

        if (damagePhase)
        {
            self.damageReduction = 2;
        }
        else
        {
            self.damageReduction = 4;   
        }

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        switch (currentState)
        {
            default:
                anim.Play("Idle");
                break;
            
            case BossState.Attacking:
                anim.Play("Attacking");
                break;
            
            case BossState.AttackCooldown:
                anim.Play("AttackCooldown");
                break;
            
            case BossState.ChoosingAttack:
                anim.Play("ChoosingAttack");
                break;
            
            case BossState.PreDamagePhase:
                anim.Play("PreDamagePhase");
                break;
            
            case BossState.DamagePhase:
                anim.Play("DamagePhase");
                break;
        }
    }

    void SwitchState(BossState state)
    {
        timer = 0;
        currentState = state;

        if (currentState == BossState.ChoosingAttack) StartCoroutine(ChooseAttackPhase());
        else if (currentState == BossState.Attacking)
        {
            nextAttack.UseAttack();
            nextAttackTime = Random.Range(nextAttack.minCooldown, nextAttack.maxCooldown);

            attacksDone++;
        }
    }

    void EnterPreDamagePhase()
    {
        attacksDone = 0;
        currentState = BossState.PreDamagePhase;
            
        // Spawn circles
        handler = Instantiate(bossCirclePrefab, transform.position, quaternion.identity, transform);
        handler.Initialize(initialBossCircles);
        handler.OnCirclesDestroyed.AddListener(EnterDamagePhase);

        Invoke("CancelPreDamagePhase", preDamagePhaseLength);
        
        roomSpawner.StartSpawner();
    }

    void CancelPreDamagePhase()
    {
        Destroy(handler.gameObject);
        handler = null;

        SwitchState(BossState.AttackCooldown);
        
        roomSpawner.StopSpawner();
    }
    
    void EnterDamagePhase()
    {
        Destroy(handler);
        handler = null;
        
        CancelInvoke("CancelPreDamagePhase");
        SwitchState(BossState.DamagePhase);
        
        // screenshake
        CameraShakerHandler.Shake(damagePhaseStartShake);
        
        // lower damage reduc
        damagePhase = true;

        Invoke("EndDamagePhase", damagePhaseLength);

        damagePhaseStartHealth = self.health;
    }

    void EndDamagePhase()
    {
        CancelInvoke("EndDamagePhase");
        
        damagePhase = false;
        attacksDone = 0;    

        SwitchState(BossState.AttackCooldown);
        nextAttackTime = postDamagePhaseCooldown;

        roomSpawner.StopSpawner();
        
        for (int i = 0; i < itemSpawners.Length; i++)
        {
            itemSpawners[i].SpawnItems();
        }
    }
    
    BossAttack ChooseAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size * 1.5f, 0, LayerMask.GetMask("Player"));

        if (colliders.Length > 0)
        {
            return closeRangeAttack;
        }

        if (attacksBeforeSpecial <= 0)
        {
            int randomIndex = Random.Range(0, specialAttacks.Length);
            attacksBeforeSpecial = Random.Range(minAmountForSpecial, maxAmountForSpecial);
            return specialAttacks[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, randomAttacks.Length);
            attacksBeforeSpecial--;
            return randomAttacks[randomIndex];
        }
    }

    IEnumerator ChooseAttackPhase()
    {  
        spawner.UseAttack();

        yield return new WaitForSeconds(chooseBufferTime);
        
        nextAttack = ChooseAttack();
        
        SwitchState(BossState.Attacking);
    }
    
    [System.Serializable]
    public enum BossState
    {
        ChoosingAttack,
        Attacking,
        AttackCooldown,
        PreDamagePhase,
        DamagePhase,
        HealingPhase,
        Idle,
    }
}
