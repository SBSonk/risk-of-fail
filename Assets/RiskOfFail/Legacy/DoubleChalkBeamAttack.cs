using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using Unity.Mathematics;
using UnityEngine;

public class DoubleChalkBeamAttack : BossAttack
{
    [SerializeField] private float beamTime = 15;
    [SerializeField] private Transform beamLeft;
    [SerializeField] private Transform beamRight;
    [SerializeField] private float beamRotateSpeed = 10;
    [SerializeField] private BossAttack spawningAttack;

    private Animator anim;
    private Transform player;
    private bool tracking = false;
    private float dir = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = PlayerStatus.instance.transform;
    }

    private void Update()
    {
        if (tracking)
        {
            beamLeft.Rotate(0, 0, beamRotateSpeed * dir * Time.deltaTime);
            beamRight.rotation = Quaternion.Euler(0, 0, -beamLeft.rotation.eulerAngles.z);
        }
    }

    public override void UseAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        beamLeft.rotation = Quaternion.Euler(0, 0, -90);
        beamRight.rotation = Quaternion.Euler(0, 0, -beamLeft.rotation.eulerAngles.z);

        anim.Play("DoubleChalkBeam");
        
        yield return new WaitForSeconds(1);
        
        tracking = true;
        
        Vector2 directionToPlayer = (player.position + new Vector3(0, 0.5f)) - transform.position;
        dir = Mathf.Sign(Vector2.Dot(transform.up, directionToPlayer));
        
        int rotations = 2;
        for (int i = 0; i < rotations * 2; i++)
        {
            spawningAttack.UseAttack();
        
            yield return new WaitForSeconds(beamTime / (rotations * 2));
        }
        
        anim.Play("DoubleChalkBeamRetract");
        tracking = false;
    }
}
