using RiskOfFail.Combat.Enums;

namespace RiskOfFail.Combat.Interfaces
{
	public interface IOnHit
	{
		public void OnHit(float damage, float stunTime, DamageTypeFlag killFlag);
	}
}