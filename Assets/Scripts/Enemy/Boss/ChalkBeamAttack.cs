using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkBeamAttack : BossAttack
{
    [SerializeField] private float beamTime = 15;
    [SerializeField] private Transform beam;
    [SerializeField] private float beamRotateSpeed = 10;
    [SerializeField] private BossAttack spawningAttack;
    
    private Animator anim;
    private Transform player;
    private bool tracking = false;
    private float dir = 1;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = PlayerStatus.player.transform;
    }

    private void Update()
    {
        if (tracking)
        {
            beam.Rotate(0, 0, beamRotateSpeed * dir * Time.deltaTime);
        }
    }

    public override void UseAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        beam.rotation = Quaternion.Euler(0, 0, 180);
        
        anim.Play("ChalkBeam");
        
        yield return new WaitForSeconds(1);
        
        tracking = true;
        
        Vector2 directionToPlayer = (player.position + new Vector3(0, 0.5f)) - transform.position;
        dir = Mathf.Sign(Vector2.Dot(transform.right, directionToPlayer));

        int rotations = 2;
        for (int i = 0; i < rotations * 2; i++)
        {
            spawningAttack.UseAttack();
        
            yield return new WaitForSeconds(beamTime / (rotations * 2));
        }
        
        anim.Play("ChalkBeamRetract");
        tracking = false;
    }
}
