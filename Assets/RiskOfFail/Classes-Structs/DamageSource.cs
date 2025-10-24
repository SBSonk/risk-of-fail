using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Damage Source", menuName = "DamageSource")]
public class DamageSource : ScriptableObject
{
    public float damage;
    public float stunTime;
}