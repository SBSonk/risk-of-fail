using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    [SerializeField] Collider2D _collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collider.enabled = true;
    }
}
