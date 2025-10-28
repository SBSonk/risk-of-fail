using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Player.Movement
{
	public abstract class MovementState
	{
		public PlayerHSMovement stateMachine { protected get; set; }
		public MovementState parent { protected get; set; }

		public virtual void OnEnter() {}
		public virtual void OnExit() {}
		public virtual void LogicUpdate() {}
		public virtual void PhysicsUpdate() {}

		public virtual bool HandleInput(InputAction.CallbackContext ctx)
		{
			return parent != null && parent.HandleInput(ctx);
		}
	}
}