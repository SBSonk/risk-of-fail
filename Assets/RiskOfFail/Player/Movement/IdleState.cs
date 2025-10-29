using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RiskOfFail.Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Player.Movement
{
	public class IdleState : PlayerMovementState
	{
		public override void OnEnter()
		{
			player.rb.linearVelocity = Vector2.zero;
		}

		public override void OnInput(InputAction.CallbackContext ctx)
		{
			if (ctx.action.name == "Move")
			{
				stateMachine.SwitchState("BaseMove");
			}
		}
	}
}