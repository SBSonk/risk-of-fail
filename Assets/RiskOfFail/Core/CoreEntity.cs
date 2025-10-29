using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Core
{
	public abstract class CoreEntity : MonoBehaviour
	{
		public Rigidbody2D rb { protected set; get; }
		public Animator anim { protected set; get; }
		
		protected virtual void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			anim = GetComponent<Animator>();
		}
	}
}