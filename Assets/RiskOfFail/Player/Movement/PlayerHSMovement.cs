using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Player.Movement
{
	public class PlayerHSMovement : MonoBehaviour
	{
		[Header("Movement")]
		public float moveSpeed = 13;
		public float stunSpeedMultiplier = .25f;

		[Header("State Controller")]
		public Dictionary<string, MovementState> movementStates = new Dictionary<string, MovementState>();
		
		private PlayerInput playerInput;

		#region State Variables

		private MovementState _currentState;

		#endregion

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			
			InitializeStates();
		}

		private void OnEnable()
		{
			playerInput.onActionTriggered += OnActionTriggered;
		}

		private void OnDisable()
		{
			playerInput.onActionTriggered -= OnActionTriggered;
		}

		void InitializeStates()
		{
			movementStates[nameof(IdleState)]
		}

		public void SwitchState(MovementState state)
		{
			
		}

		void OnActionTriggered(InputAction.CallbackContext ctx)
		{
			_currentState.HandleInput(ctx);
		}
	}
}