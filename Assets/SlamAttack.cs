using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SlamAttack : BossAttack
    {
        [SerializeField] private DamageSource damageSource;
        [SerializeField] private float attackDelay;
        [SerializeField] private float knockbackAmount;

        private Animator anim;
        private Collider2D col;

        private void Start()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<Collider2D>();
        }

        public override void UseAttack()
        {
            StartCoroutine(Attack());
        }

        IEnumerator Attack()
        {
            anim.Play("Slam", 0, 0);
            
            yield return new WaitForSeconds(attackDelay);

            Collider2D[] colliders = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size * 1.5f, 0, LayerMask.GetMask("Player"));

            foreach (Collider2D player in colliders)
            {
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<Alive>().GiveDamage(damageSource, KillFlag.AreaOfEffect);
                    player.GetComponent<Rigidbody2D>().AddForce(-(col.bounds.ClosestPoint(player.transform.position) - player.transform.position).normalized * knockbackAmount, ForceMode2D.Impulse);
                }
            }
        }
    }
