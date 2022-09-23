using UnityEngine;

[CreateAssetMenu(menuName = "New Ammo Drop", fileName = "Ammo Drop")] [System.Serializable]
public class AmmoDrops : ScriptableObject
{
    public Weapon typeToGive;
    public int min, max;

    [Header("Visuals")]
    public Sprite sprite;
    public Color spriteColor = Color.white;
    public Color backgroundColor = Color.red;
}
