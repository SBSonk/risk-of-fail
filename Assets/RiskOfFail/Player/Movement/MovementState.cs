using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Player.Movement
{
	public abstract class MovementState
	{
		protected PlayerHSMovement movement;
		protected MovementState parent;

		public MovementState(PlayerHSMovement movement, MovementState parent = null)
		{
			this.movement = movement;
			this.parent = parent;
		}
		
		public virtual void OnEnter() {}
		public virtual void OnExit() {}
		public virtual void LogicUpdate() {}
		public virtual void PhysicsUpdate() {}
	}
}