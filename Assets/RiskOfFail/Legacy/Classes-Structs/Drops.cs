using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[CreateAssetMenu(fileName = "New Drop", menuName = "Drops/Drop")]
public class Drops : ScriptableObject
{
    public GameObject obj;

    [Range(0, 100)] public int dropChance = 100;

    public int minDropAmount, maxDropAmount = 1;

    public void Spawn(Vector3 pos, out GameObject[] drops)
    {
        drops = null;

        if (Random.value * 100 >= dropChance) return;

        drops = new GameObject[Random.Range(minDropAmount, maxDropAmount + 1)];
        for (var i = 0; i < drops.Length; i++)
        {
            drops[i] = Instantiate(obj);
            drops[i].transform.position = pos;

            var randDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            drops[i].GetComponent<Rigidbody2D>().AddForce(randDir * Random.Range(3f, 4f), ForceMode2D.Impulse);
        }
    }

    public void Spawn(Vector3 pos, out GameObject[] drops, Vector3 playerPosition)
    {
        drops = null;

        if (Random.value * 100 >= dropChance) return;

        drops = new GameObject[Random.Range(minDropAmount, maxDropAmount + 1)];
        for (var i = 0; i < drops.Length; i++)
        {
            drops[i] = Instantiate(obj);
            drops[i].transform.position = pos;

            var randDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            var dirToPlayer = playerPosition - pos;
            drops[i].GetComponent<Rigidbody2D>().AddForce((randDir + dirToPlayer).normalized * Random.Range(3f, 4f),
                ForceMode2D.Impulse);
        }
    }

    public void Spawn(Vector3 pos)
    {
        Spawn(pos, out _);
    }
}