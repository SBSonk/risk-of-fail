using System;
using RiskOfFail.Combat.Enums;

namespace RiskOfFail.Combat.Interfaces
{
	public interface IOnHeal
	{
		public void OnHeal(float damage, DamageTypeFlag killFlag);
	}
}