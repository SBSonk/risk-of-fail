using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Color negative, positive, immune;
    [SerializeField] private float lifetime = 1; // How long before it gets destroyed
    [SerializeField] private TextMeshPro damageNumber;
    [SerializeField] private Animator animator;

    public void Initialize(float damageAmount)
    {
        // Change text
        damageNumber.text = Mathf.RoundToInt(damageAmount).ToString();

        // Change color
        if (damageAmount > 0)
        {
            damageNumber.color = positive;
        }
        else if (damageAmount < 0)
        {
            damageNumber.color = negative;
        }
        else
        {
            damageNumber.color = immune;
            damageNumber.text = "IMMUNE";
        }

        Destroy(gameObject, lifetime);
    }
}