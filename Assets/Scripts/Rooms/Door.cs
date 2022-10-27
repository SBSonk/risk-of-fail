using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float toggleDelay = 0.5f;

    public void ToggleDoor(bool val)
    {
        StartCoroutine(Toggle(val));
    }

    IEnumerator Toggle(bool val)
    {
        yield return new WaitForSeconds(toggleDelay);

        GetComponent<SpriteRenderer>().enabled = val;
        GetComponent<Collider2D>().enabled = val;
    }
}
