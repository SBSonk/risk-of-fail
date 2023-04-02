using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundCounter;

    private ZSpawning2 spawner;

    private void Start()
    {
        spawner = FindObjectOfType<ZSpawning2>();
    }

    public void UpdateRoundCounter()
    {
        roundCounter.text = spawner.round.ToString();
    }
}
