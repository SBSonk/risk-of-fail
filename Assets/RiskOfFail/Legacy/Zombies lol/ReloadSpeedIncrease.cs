using RiskOfFail.Combat;

public class ReloadSpeedIncrease : HealthIncrease
{
    protected override void Effect()
    {
        
        PlayerStatus.player.pShooting.reloadMultiplier = 1.5f;
    }
}