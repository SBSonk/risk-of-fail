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
	public class DashState : PlayerMovementState
	{
		// Inputs
		private Vector2 _input;

		public override void OnEnter()
		{
			GetDirectionalInput();

			player.StartCoroutine(DashCoroutine());
		}

		IEnumerator DashCoroutine()
		{
			float t = 0;
			Vector2 startPos = player.transform.position;
			Vector2 endPos = startPos + (_input * player.dashDistance);
			
			while (t < player.dashTime)
			{
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;

				float time = player.dashCurve.Evaluate(t / player.dashTime);
				player.rb.MovePosition(Vector2.Lerp(startPos, endPos, time));
			}
			
			player.rb.MovePosition(endPos);
			
			stateMachine.SwitchState("BaseMove");
		}
		
		void GetDirectionalInput()
		{
			_input = playerInput.actions["Move"].ReadValue<Vector2>();
		}
	}
}