using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public abstract class BossAttack : MonoBehaviour
{
    public abstract void UseAttack();
}