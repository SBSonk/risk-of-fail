using RiskOfFail.Combat;

public class DamageIncrease : HealthIncrease
{
    protected override void Effect()
    {
        PlayerStatus.player.pShooting.damageMultiplier = 2;
    }
}