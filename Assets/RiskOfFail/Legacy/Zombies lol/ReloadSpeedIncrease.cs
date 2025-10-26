using RiskOfFail.Combat;

public class ReloadSpeedIncrease : HealthIncrease
{
    protected override void Effect()
    {
        
        PlayerStatus.instance.shooting.reloadMultiplier = 1.5f;
    }
}