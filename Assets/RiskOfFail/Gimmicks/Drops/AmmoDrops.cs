using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Drops/Ammo Drop", fileName = "New Ammo Drop")]
[Serializable]
public class AmmoDrops : ScriptableObject
{
    public Weapon typeToGive;
    public int min, max;

    [Header("Visuals")] public Sprite sprite;

    public Color spriteColor = Color.white;
    public Color backgroundColor = Color.red;
}