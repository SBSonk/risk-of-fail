using RiskOfFail.Combat;
using Unity.Mathematics;
using UnityEngine;

public class EraserSlamAttack : BossAttack
{
    [SerializeField] private LerpFollow eraserPrefab;

    private Transform player;

    private void Start()
    {
        player = PlayerStatus.instance.transform;
    }

    public override void UseAttack()
    {
        var eraser = Instantiate(eraserPrefab, player.position, quaternion.identity);
        eraser.target = player;
    }
}