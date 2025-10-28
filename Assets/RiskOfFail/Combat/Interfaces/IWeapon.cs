using System;

namespace RiskOfFail.Combat.Interfaces
{
	public interface IWeapon
	{
		public void OnEquip();
		public void OnUnequip();

		public void HandleLogic();
	}
}