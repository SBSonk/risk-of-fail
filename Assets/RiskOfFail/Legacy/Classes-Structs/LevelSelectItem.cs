using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/LevelSelectItem", fileName = "New Level Select Item")]
[Serializable]
public class LevelSelectItem : ScriptableObject
{
    public int buildIndex;

    [TextArea] public string description;

    public Sprite preview;
    public bool followLoadout;
}