using System;
using RiskOfFail.Combat.ScriptableObjects;

namespace RiskOfFail.Combat.Classes
{
	[Serializable]
	public class ActiveStatusEffect
	{
		public StatusEffectBase effect;
		public int secondsLeft;

		public ActiveStatusEffect(StatusEffectBase effect)
		{
			this.effect = effect;
			secondsLeft = effect.duration;
		}
	}
}