using System;

namespace RiskOfFail.Combat.Enums
{
	[Serializable]
	public enum DamageTypeFlag
	{
		Melee,
		Ranged,
		Self,
		AreaOfEffect,
		LevelPassed,
		Despawn
	}
}