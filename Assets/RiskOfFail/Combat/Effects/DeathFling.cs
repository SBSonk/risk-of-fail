using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RiskOfFail.Combat.Effects
{
	public class DeathFling : MonoBehaviour, IOnDeath
	{
		[Header("Death Fling")]
        public GameObject deathFlingPrefab;
		
		public void OnDeath(DamageTypeFlag killFlag)
		{
			if (killFlag == DamageTypeFlag.Despawn) return;
			
			if (deathFlingPrefab)
			{
				var rb = GetFlingObject().GetComponent<Rigidbody2D>();

				rb.AddForce(new Vector3(Random.Range(1, -1f) * Random.Range(5, 10f), Random.Range(2.5f, 10f)),
					ForceMode2D.Impulse);
				rb.AddTorque(-Mathf.Sign(rb.linearVelocity.x) * Random.Range(5, 10f), ForceMode2D.Impulse);
			}
		}

		GameObject GetFlingObject() => Instantiate(deathFlingPrefab, transform.position, Quaternion.identity); // Placeholder for object pool
	}
}