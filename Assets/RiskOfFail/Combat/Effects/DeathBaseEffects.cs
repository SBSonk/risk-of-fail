using System;
using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Combat.Effects
{
	public class DeathBaseEffects : MonoBehaviour, IOnDeath
	{
		[Header("Death Effects")]
		public AudioClip deathSound;
		public GameObject deathParticles;
		
		public void OnDeath(DamageTypeFlag killFlag)
		{
			if (deathSound)
			{
				var deathSFX = GetDeathSound();
				
				deathSFX.PlayOneShot(deathSound);
			}

			if (deathParticles) GetDeathParticles();
		}
		
		GameObject GetDeathParticles() 
		{
			return Instantiate(deathParticles, transform.position, Quaternion.identity);
		}
		
		AudioSource GetDeathSound()
		{
			var spawned = new GameObject($"Death Sound ({deathSound.name})");
			spawned.AddComponent<AudioSource>();
			
			return spawned.GetComponent<AudioSource>();
		}
	}
}