using TMPro;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundCounter;

    private ZSpawning2 spawner;

    private void Start()
    {
        spawner = FindObjectOfType<ZSpawning2>();
    }

    public void UpdateRoundCounter()
    {
        roundCounter.text = spawner.round.ToString();
    }
}