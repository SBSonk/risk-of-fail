using UnityEngine;

[System.Serializable]
public struct Drops
{
    public GameObject obj;
    [Range(0, 100)]
    public int dropChance;
    public int dropAmount, minDropAmount;
}
