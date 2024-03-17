using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillArea : MonoBehaviour
{
    public UnityEvent OnKill;   
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        throw new NotImplementedException();
    }
}
