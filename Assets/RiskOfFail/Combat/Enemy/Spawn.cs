using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float rate1;
    public GameObject[] e1;
    public int waves1;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnEnemy1", rate1, rate1);
    }

    private void SpawnEnemy1() //WW spawn
    {
        for (var i = 0; i < waves1; i++)
            Instantiate(e1[Random.Range(0, e1.Length)], new Vector3(Random.Range(-15, 15), -20, 0),
                Quaternion.identity);
    }
}