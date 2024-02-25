using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData pData;

    public static GameManager main;
    
    void Awake()
    {
        // Destroy duplicates
        main = this;
        
        Enemy.globalEnemyHealthScale = 1;
    }
}
