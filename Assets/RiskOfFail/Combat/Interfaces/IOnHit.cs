using RiskOfFail.Combat.Enums;

namespace RiskOfFail.Combat.Interfaces
{
	public interface IOnHit
	{
		public void OnHit(DamageTypeFlag killFlag);
	}
}