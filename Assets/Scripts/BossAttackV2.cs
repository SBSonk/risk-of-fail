using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityRandom = UnityEngine.Random;

public abstract class BossAttackV2 : MonoBehaviour
{
    public UnityEvent OnAttackEnd;

    public abstract IEnumerator Attack();

    public virtual void CancelAttack() {}
}