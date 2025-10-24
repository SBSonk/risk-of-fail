using UnityEngine;

public class Indicator : MonoBehaviour
{
    public void play()
    {
        GetComponent<Animator>().Play("Explosion Indicator");
    }
}