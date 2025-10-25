using RiskOfFail.Combat;
using UnityEngine;

public class HealthIncrease : MonoBehaviour
{
    public Sprite perkIcon;

    public void Purchase()
    {
        Effect();

        if (TryGetComponent<Animator>(out var anim)) anim.Play("Bought");

        PerkSlots.main.AddIcon(perkIcon);
    }

    protected virtual void Effect()
    {
        PlayerStatus.player.maxHealth += 25;
        //PlayerStatus.player.health = PlayerStatus.player.maxHealth;
    }
}