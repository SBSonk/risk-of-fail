using UnityEngine;

[System.Serializable]   
public class StatusEffect : ScriptableObject
{
    public string statusName;
    public Sprite statusIcon;

    [Range(0, 100)]
    public int inflictChance = 100;

    public virtual void OnStatusTick(Alive target)
    {

    }
}
