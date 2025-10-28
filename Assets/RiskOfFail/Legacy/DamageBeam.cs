using System;
using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;

public class DamageBeam : MonoBehaviour
{
    [SerializeField] private DamageSource _damageSource;
    [SerializeField] private float knockbackAmount = 10;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private float lineLerp = 25;

    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, new Vector2(3, 4), CapsuleDirection2D.Vertical, 0, transform.up, 50, LayerMask.GetMask("Environment"));

        if (hit)
        {
            line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), new Vector3(0, transform.InverseTransformPoint(hit.point).y, 0), lineLerp * Time.deltaTime));

            // Calculate the collider size
            float colliderHeight = Vector3.Distance(transform.position, hit.point);
            col.size = new Vector2(col.size.x, colliderHeight);
            col.offset = new Vector2(0, colliderHeight/2);
        }
        else
        {
            line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), transform.position - new Vector3(0, -50), lineLerp * Time.deltaTime));
            col.size = new Vector2(2.15f, 50f);
            col.offset = new Vector2(0, 25);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<PlayerStatus>();
            
            player.GiveDamage(_damageSource, DamageTypeFlag.AreaOfEffect);
            player.GetComponent<Rigidbody2D>().AddForce(-(col.bounds.ClosestPoint(player.transform.position) - player.transform.position).normalized * knockbackAmount, ForceMode2D.Impulse);
        }
    }
}
