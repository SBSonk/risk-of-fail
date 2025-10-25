using UnityEngine;

public class TeleportPlayerTo : MonoBehaviour
{
    [SerializeField] private Transform endPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.transform.position = endPos.position;
    }
}