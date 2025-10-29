using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Core.Utilities
{
	public class StateMachine
	{
		public Dictionary<string, State> _states = new Dictionary<string, State>();

		private State _currentState;

		public void RegisterState(string name, State state)
		{
			_states.Add(name, state);
		}

		public void SwitchState(string stateName)
		{
			_currentState?.OnExit();

			if (_states.TryGetValue(stateName, out var state))
			{
				_currentState = state;
				_currentState.OnEnter();

				return;
			}
			
			Debug.LogError($"State {GetType().Name}.{stateName} does not exist.");
		}

		public void OnUpdate()
		{
			_currentState?.OnUpdate();
		}

		public void OnFixedUpdate()
		{
			_currentState?.OnFixedUpdate();
		}

		public void OnInput(InputAction.CallbackContext ctx)
		{
			_currentState?.OnInput(ctx);
		}

		public string GetStateName() => _currentState.GetType().ToString();
	}
}