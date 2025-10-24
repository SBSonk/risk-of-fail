using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class BossAttackV2 : MonoBehaviour
{
    public UnityEvent OnAttackEnd;

    public abstract IEnumerator Attack();

    public virtual void CancelAttack()
    {
    }
}