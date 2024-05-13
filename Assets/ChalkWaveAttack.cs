using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChalkWaveAttack : BossAttackV2
{
    public Transform boss;
    public float maxX = 20;

    [Header("Scaling")] 
    public Vector3 normalScale = new Vector3(2, 2, 1);
    public Vector3 attackScale = new Vector3(4, 4, 1);

    [Header("Choosing Phase")] public float windUpTime = 2;
    //public int choosingPhaseFakes = 2;
    public float choosingPhaseTime = 3;
    //public float choosingPhaseHoverTime = 3;

    [Header("Attack")]
    public int waves = 4;
    public float waveSpeed = 3;
    public float timeBetweenWaves = 1;
    public Vector3 waveOffset;
    public Rigidbody2D wavePrefab;

    private float startX;

    private void Start()
    {
        if (!boss) boss = transform;
    }

    public override IEnumerator Attack()
    {
        startX = boss.position.x;

        boss.DOScale(attackScale, windUpTime);
        yield return new WaitForSeconds(windUpTime);
        
        // Attack
        for (int i = 0; i < waves; i++)
        {
            boss.DOMoveX(startX + Random.Range(-maxX, maxX), choosingPhaseTime);
            yield return new WaitForSeconds(choosingPhaseTime);
            
            Rigidbody2D wave = Instantiate(wavePrefab, boss.position + waveOffset, transform.rotation);
            wave.velocity = -wave.transform.up.normalized * waveSpeed;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        boss.DOMoveX(startX, choosingPhaseTime);
        yield return new WaitForSeconds(choosingPhaseTime);
        boss.DOScale(normalScale, windUpTime);
        print("test");
        OnAttackEnd?.Invoke();
    }

    public override void CancelAttack()
    {
        boss.DOMoveX(startX, choosingPhaseTime);
        boss.DOScale(normalScale, windUpTime);
    }
}
