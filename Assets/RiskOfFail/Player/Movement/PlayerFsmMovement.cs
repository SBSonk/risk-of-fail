using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RiskOfFail.Combat.Effects;
using RiskOfFail.Core;
using RiskOfFail.Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Player.Movement
{
	[RequireComponent(typeof(HasStun))]
	public class PlayerFsmMovement : CoreEntity
	{
		[Header("Movement")]
		public float moveSpeed = 13;
		public float runSpeedMultiplier = 1.25f;
		public float stunSpeedMultiplier = .5f;
		public float moveDampening = .25f;

		[Header("Dash")]
		public float dashDistance = 3f;
		public float dashTime = .5f;
		public AnimationCurve dashCurve;

		[Header("State Controller")]
		public StateMachine stateMachine;

		[Header("Controls")]
		public float deadZone = .1f;
		
		public PlayerInput playerInput;

		public HasStun stunStatus;
		
		#if UNITY_EDITOR

		[ReadOnly, SerializeField]
		private string DEBUG_CurrentState;
		
		#endif

		private void Awake()
		{
			base.Awake();
			
			playerInput = GetComponent<PlayerInput>();
			stunStatus = GetComponent<HasStun>();
			
			RegisterStates();
		}

		private void OnEnable()
		{
			playerInput.onActionTriggered += OnActionTriggered;
		}

		private void OnDisable()
		{
			playerInput.onActionTriggered -= OnActionTriggered;
		}

		private void Update()
		{
			stateMachine.OnUpdate();
			
			#if UNITY_EDITOR

			DEBUG_CurrentState = stateMachine.GetStateName();

			#endif
		}

		private void FixedUpdate()
		{
			stateMachine.OnFixedUpdate();
		}

		void RegisterStates()
		{
			stateMachine = new StateMachine();
			
			stateMachine.RegisterState("Idle", new IdleState() {stateMachine = stateMachine, player = this, playerInput = playerInput});
			stateMachine.RegisterState("BaseMove", new BaseMoveState() {stateMachine = stateMachine, player = this, playerInput = playerInput});
			stateMachine.RegisterState("Dash", new DashState() {stateMachine = stateMachine, player = this, playerInput = playerInput});
			
			stateMachine.SwitchState("Idle");
		}
		
		void OnActionTriggered(InputAction.CallbackContext ctx)
		{
			stateMachine.OnInput(ctx);
		}
	}
}