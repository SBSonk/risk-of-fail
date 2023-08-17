using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChalkQuakeSegment : MonoBehaviour
{
    [SerializeField] private float windUpTime = 3f, hideTime = 1f;
    [SerializeField] private DamageSource _damageSource;
    [SerializeField] private Transform spike;

    private Animator anim;

    private PlayerStatus player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
        spike.rotation = quaternion.identity;
    }

    public void StartAttack(float time)
    {
        Invoke("Attack", time);
    }

    void Attack() => StartCoroutine(StartDamageAnimation());

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) player = col.GetComponent<PlayerStatus>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) player = null;
    }

    IEnumerator StartDamageAnimation()
    {
        // play anim
        anim.Play("Show");

        yield return new WaitForSeconds(windUpTime);

        // do damage
        if (player)
            player.GiveDamage(_damageSource.damage, _damageSource.stunTime, KillFlag.AreaOfEffect, null,
                _damageSource.useRawDamage);
        
        // play hideAnim
        anim.Play("Hide");

        yield return new WaitForSeconds(hideTime);

        Destroy(gameObject);
    }
}
