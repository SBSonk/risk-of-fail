using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="New Damage Source", menuName ="DamageSource")]
public class DamageSource : ScriptableObject
{
    public bool useRawDamage;
    public float damage;
    public float stunTime;
}
