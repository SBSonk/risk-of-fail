using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using UnityEngine;

public class DropObject : MonoBehaviour, IOnDeath
{
    public Drops[] drops;

    public void OnDeath(DamageTypeFlag killFlag)
    {
        foreach (var d in drops) d.Spawn(transform.position);
    }
}