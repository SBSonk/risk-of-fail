using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    public float minCooldown, maxCooldown;
    public abstract void UseAttack();
}