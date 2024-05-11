using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] Color negative, positive, immune;
    [SerializeField] float lifetime = 1; // How long before it gets destroyed
    [SerializeField] TextMeshPro damageNumber;
    [SerializeField] Animator animator;

    public void Initialize(float damageAmount)
    {
        // Change text
        damageNumber.text = Mathf.RoundToInt(damageAmount).ToString();

        // Change color
        if (damageAmount > 0) damageNumber.color = positive;
        else if (damageAmount < 0) damageNumber.color = negative;
        else
        {
            damageNumber.color = immune;
            damageNumber.text = "IMMUNE";
        }

        Destroy(gameObject, lifetime);
    }
}
