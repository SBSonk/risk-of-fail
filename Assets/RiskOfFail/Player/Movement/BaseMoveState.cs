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
	public class BaseMoveState : PlayerMovementState
	{
		private bool _running;
		private Vector2 _refVelocity;
		
		// Inputs
		private Vector2 _input;
		private bool _dashHeld;

		public override void OnEnter()
		{
			_running = false;
		}

		public override void OnUpdate()
		{
			GetDynamicInputs();
			
			_running = _dashHeld;

			if (_input.magnitude < player.deadZone) stateMachine.SwitchState("Idle");
		}

		public override void OnFixedUpdate()
		{
			Vector2 targetVel = _input * GetTargetSpeed();
			player.rb.linearVelocity = Vector2.SmoothDamp(player.rb.linearVelocity, targetVel, ref _refVelocity,
				player.moveDampening);
		}

		public override void OnInput(InputAction.CallbackContext ctx)
		{
			if (ctx.action.name == "Dash" && !player.stunStatus.isStunned)
			{
				if (ctx.started) stateMachine.SwitchState("Dash");
			}
		}

		void GetDynamicInputs()
		{
			_input = playerInput.actions["Move"].ReadValue<Vector2>();
			_dashHeld = playerInput.actions["Dash"].IsInProgress();
		}

		float GetTargetSpeed()
		{
			float targetSpeed = player.moveSpeed;
			if (_running) targetSpeed *= player.runSpeedMultiplier;

			if (player.stunStatus.isStunned) targetSpeed *= player.stunSpeedMultiplier;

			return targetSpeed;
		}
	}
}