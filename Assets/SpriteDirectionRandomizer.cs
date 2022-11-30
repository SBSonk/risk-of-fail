using UnityEngine;

// Funny script to add variety to repeating tiles
public class SpriteDirectionRandomizer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().flipX = Random.value > 0.5f;
    }
}
