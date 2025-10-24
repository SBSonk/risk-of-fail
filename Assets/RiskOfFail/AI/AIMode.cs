using System;

namespace RiskOfFail.AI
{
    [Serializable]
    public enum AIMode
    {
        pathing = 0,
        shooting = 1,
        repositioning = 2,
        pushing = 3,
        attacking = 4,
        idle = 5
    }
}