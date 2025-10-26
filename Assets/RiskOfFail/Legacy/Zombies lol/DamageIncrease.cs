using RiskOfFail.Combat;

public class DamageIncrease : HealthIncrease
{
    protected override void Effect()
    {
        PlayerStatus.instance.shooting.damageMultiplier = 2;
    }
}