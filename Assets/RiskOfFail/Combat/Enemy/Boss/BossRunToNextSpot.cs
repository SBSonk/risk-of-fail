using DG.Tweening;
using UnityEngine;

public class BossRunToNextSpot : MonoBehaviour
{
    public Transform[] wayPoints;
    public float moveSpeed;

    private int index;

    private void Start()
    {
        MoveToNext();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (var i = 0; i < wayPoints.Length; i++)
        {
            if (i + 1 < wayPoints.Length) Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);

            Gizmos.DrawSphere(wayPoints[i].position, .5f);
        }
    }

    public void MoveToNext()
    {
        index++;

        transform.DOMove(wayPoints[index].position, moveSpeed).SetSpeedBased(true).SetEase(Ease.Linear);
    }
}