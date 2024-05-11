using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnHealCanisters : MonoBehaviour
{
    public BossEnemy boss;
    public BossRoomFirstPhase attacker;
    public HealBoss canisterPrefab;

    public Transform[] canisterSpawns;
    
    public float healCanisterDelay;
    public float timeBetweenSpawns = 0.25f;

    List<HealBoss> canisters = new List<HealBoss>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < canisterSpawns.Length; i++)
        {
            Gizmos.DrawSphere(canisterSpawns[i].position, 1f);
        }
    }

    public void StartSpawnProcess() => StartCoroutine(SpawnCanisters());
    public void StartSpawnProcessIntro() => StartCoroutine(SpawnCanisters(1));

    public void DisableAllCanisters()
    {
        for (int i = 0; i < canisters.Count; i++)
        {
            canisters[i].DisableHeal();
        }
    }

    public void EnableAllCanisters()
    {
        for (int i = 0; i < canisters.Count; i++)
        {
            canisters[i].EnableHeal();
        }
    }
    
    IEnumerator SpawnCanisters(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        
        for (int i = 0; i < canisterSpawns.Length; i++)
        {
            HealBoss c = Instantiate(canisterPrefab, canisterSpawns[i].position, quaternion.identity);
            c.StartLoop(boss, healCanisterDelay);

            canisters.Add(c);
            
            // Add events
            Enemy e = c.GetComponent<Enemy>();
            e.onDeath.AddListener((_) => attacker.OnCanisterKill());
            e.onDeath.AddListener((_) => attacker.StartDamagePhase());
            e.onDeath.AddListener((_) => canisters.Remove(c));

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
