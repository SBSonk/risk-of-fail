using System.Collections;
using RiskOfFail.Combat;
using UnityEngine;
using UnityEngine.Events;

public class BossCircleHandler : MonoBehaviour
{
    [SerializeField] private Alive bossCirclePrefab;
    [SerializeField] private Transform circleParent;
    [SerializeField] private float rotateSpeed;

    public UnityEvent OnCirclesDestroyed;
    private int circlesLeft;

    private void Update()
    {
        circleParent.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    public void Initialize(int amountToSpawn)
    {
        // spawn all circles
        for (var i = 0; i < amountToSpawn; i++) circlesLeft++;

        StartCoroutine(SpawnSequence(amountToSpawn));
    }

    private IEnumerator SpawnSequence(int amountToSpawn)
    {
        // spawn all circles
        for (var i = 0; i < amountToSpawn; i++)
        {
            var circle = Instantiate(bossCirclePrefab, circleParent);
            circle.onDeath.AddListener(flag => { DestroyedCircle(); });

            circle.transform.position = circleParent.transform.position + Vector3.right * 8.5f;

            yield return new WaitForSeconds(360 / rotateSpeed / amountToSpawn);
        }
    }

    private void DestroyedCircle()
    {
        circlesLeft--;

        if (circlesLeft <= 0)
        {
            OnCirclesDestroyed?.Invoke();
            Destroy(gameObject, 5f);
        }
    }
}