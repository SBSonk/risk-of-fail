using RiskOfFail.Combat.Enums;
using UnityEngine;

namespace RiskOfFail.Combat
{
    public class QuarterlyAssessment : Enemy
    {
        public void TriggerDeath()
        {
            Death(DamageTypeFlag.Self);
        }
    }
}