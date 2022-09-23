using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseAmmoDropType : MonoBehaviour
{
    AmmoPickup pickup;
    [SerializeField] AmmoDrops[] drops;

    private void Awake()
    {
        pickup = GetComponent<AmmoPickup>();

        // Choose drop to drop
        int rand = Random.Range(0, drops.Length);

        pickup.drop = drops[rand];
    }
}
