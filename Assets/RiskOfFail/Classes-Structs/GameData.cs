using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int fPoints;
    public List<InventoryWeapon> weaponsOwned;

    public GameData(int fudgePoints = 0)
    {
        fPoints = fudgePoints;
    }
}