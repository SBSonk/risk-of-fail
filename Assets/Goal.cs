using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private int bonusPoints = 500;
    
    private bool active = true;
    private ParticleSystem confetti;

    private void Start()
    {
        confetti = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Ball>(out _) && active)
        {
            active = false;
            confetti.Play();
            
            LevelStats.main.GiveScore(bonusPoints);
        }
    }
}  
