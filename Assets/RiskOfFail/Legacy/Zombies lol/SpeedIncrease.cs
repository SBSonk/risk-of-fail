using RiskOfFail.Combat;

public class SpeedIncrease : HealthIncrease
{
    protected override void Effect()
    {
        PlayerStatus.instance.movement.moveSpeed = 8000;
        PlayerStatus.instance.movement.baseSpeed = 8000;
    }
}