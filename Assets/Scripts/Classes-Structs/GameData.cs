using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int fPoints = 0;
    public List<InventoryWeapon> weaponsOwned;

    public GameData(int fudgePoints = 0)
    {
        this.fPoints = fudgePoints;
    }
}
