using System;
using NaughtyAttributes;
using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using UnityEngine;

namespace RiskOfFail.Combat.Effects
{
	public class HasStun : MonoBehaviour, IOnHit
	{
		public bool isStunned { get; private set; }
		
		private float stunTimer;
		
		private void Update()
		{
			if (isStunned)
			{
				stunTimer -= Time.deltaTime;
				if (stunTimer <= 0)
				{
					isStunned = false;
				}
			}
		}

		public void OnHit(float damage, float stunLength, DamageTypeFlag killFlag)
		{
			isStunned = true;

			stunTimer = stunLength;
		}
	}
}