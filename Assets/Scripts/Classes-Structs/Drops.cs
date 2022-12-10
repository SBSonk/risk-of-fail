using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="New Drop", menuName = "Drop")]
public class Drops : ScriptableObject
{
    public GameObject obj;

    [Range(0, 100)]
    public int dropChance = 100;
    public int minDropAmount = 0, maxDropAmount = 1;

    public void Spawn(Vector3 pos, out GameObject[] drops)
    {
        drops = null;

        if ((Random.value * 100) >= dropChance) return;

        drops = new GameObject[Random.Range(minDropAmount, maxDropAmount+1)];
        for (int i = 0; i < drops.Length; i++)
        {
            drops[i] = Object.Instantiate(obj);
            drops[i].transform.position = pos;

            Vector3 randDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            drops[i].GetComponent<Rigidbody2D>().AddForce(randDir * Random.Range(3f, 4f), ForceMode2D.Impulse);
        }

        return;
    }

    public void Spawn(Vector3 pos)
    {
        Spawn(pos, out _);
    }
}
