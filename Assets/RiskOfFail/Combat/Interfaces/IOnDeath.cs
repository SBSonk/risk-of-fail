using RiskOfFail.Combat.Enums;

namespace RiskOfFail.Combat.Interfaces
{
	public interface IOnDeath
	{
		public void OnDeath(DamageTypeFlag killFlag);
	}
}