using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.UI.World
{
	public class WorldHudBar : MonoBehaviour
	{
		[Range(0, 1)] public float fillAmount { private get; set; } = 1;

		public float lerpSpeed = 100f;

		public Transform pivot;
		private float targetFillAmount;
		private void FixedUpdate()
		{
			targetFillAmount = Mathf.Lerp(targetFillAmount, fillAmount, lerpSpeed * Time.fixedDeltaTime);
			pivot.localScale = new Vector3(targetFillAmount, 1, 1);
		}
	}
}