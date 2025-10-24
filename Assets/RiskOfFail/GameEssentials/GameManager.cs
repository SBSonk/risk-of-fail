using RiskOfFail.Combat;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public GameData pData;

    private void Awake()
    {
        // Destroy duplicates
        main = this;

        Enemy.globalEnemyHealthScale = 1;
    }
}