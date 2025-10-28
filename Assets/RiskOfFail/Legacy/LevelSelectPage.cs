using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPage : MonoBehaviour
{
    public LevelSelectButton firstLevelInPage;

    private void OnEnable()
    {
        firstLevelInPage.SelectLevel(0);
    }
}
