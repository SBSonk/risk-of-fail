using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using RiskOfFail.UI.World;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Combat.Effects
{
	[RequireComponent(typeof(Alive))]
	public class HasHealthbar : MonoBehaviour, IOnHit, IOnDeath
	{
		public WorldHudBar healthBarPrefab;
		
		[Header("Visuals")]
		public float topYPadding = .25f;
		public float fadeTime = .25f;
		public Ease fadeEase = Ease.InOutQuart;
		SpriteRenderer[] healthbarSprites;
		
		private float stayTime = 3f;
		private float lastHitTime;
		private bool shown;
		
		private Alive trackedAlive;
		private WorldHudBar activeHealthBar;

		private void Awake()
		{
			trackedAlive = GetComponent<Alive>();
		}

		private void Start()
		{
			InitializeHealthBar();
		}
		private void Update()
		{
			if (!activeHealthBar) return;
			
			if (shown && lastHitTime + stayTime < Time.time)
			{
				shown = false;
				FadeOut();
			}
		}

		public void OnDeath(DamageTypeFlag killFlag)
		{
			ReturnHealthBar();
		}

		public void OnHit(float damage, float stunTime, DamageTypeFlag killFlag)
		{
			activeHealthBar.fillAmount = trackedAlive.health / trackedAlive.maxHealth;

			if (!shown)
			{
				shown = true;
				FadeIn();
			}
			
			lastHitTime = Time.time;
		}
		
		WorldHudBar GetHealthBar() => Instantiate(healthBarPrefab, transform.position, Quaternion.identity);

		Vector2 GetPositionOffset()
		{
			var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
			if (sr == null || sr.sprite == null)
				return Vector2.zero;

			// Get local sprite bounds
			float topY = sr.sprite.bounds.max.y * sr.transform.localScale.y;

			return new Vector2(0, topY + topYPadding);
		}

		private void InitializeHealthBar()
		{
			activeHealthBar = GetHealthBar();
			LerpFollow lerpFollow = activeHealthBar.GetComponent<LerpFollow>();
			lerpFollow.target = trackedAlive.transform;
			lerpFollow.offset = GetPositionOffset();

			healthbarSprites = activeHealthBar.GetComponentsInChildren<SpriteRenderer>();
			
			shown = false;
			FadeOut();
		}
		
		void ReturnHealthBar()
		{
			Destroy(activeHealthBar.gameObject);
			activeHealthBar = null;
		}

		void FadeOut()
		{
			foreach (SpriteRenderer sr in healthbarSprites)
			{
				sr.DOFade(0, fadeTime).SetEase(fadeEase);
			}
		}

		void FadeIn()
		{
			foreach (SpriteRenderer sr in healthbarSprites)
			{
				sr.DOFade(1, fadeTime).SetEase(fadeEase);
			}
		}
	}
}