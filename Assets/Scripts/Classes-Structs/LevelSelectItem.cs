using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/LevelSelectItem", fileName = "New Level Select Item")][System.Serializable]
public class LevelSelectItem : ScriptableObject
{
    public int buildIndex;
    
    [TextArea] public string description;

    public Sprite preview;
}
