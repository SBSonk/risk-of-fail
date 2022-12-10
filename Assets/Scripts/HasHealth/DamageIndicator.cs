using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] Color negative, positive;
    [SerializeField] float lifetime = 1; // How long before it gets destroyed
    [SerializeField] TextMeshPro damageNumber;
    [SerializeField] Animator animator;

    public void Initialize(float damageAmount)
    {
        // Change text
        damageNumber.text = Mathf.RoundToInt(damageAmount).ToString();

        // Change color
        if (Mathf.Sign(damageAmount) > 0) damageNumber.color = positive;
        else damageNumber.color = negative;

        Destroy(gameObject, lifetime);
    }
}
