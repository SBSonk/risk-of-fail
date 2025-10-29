using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RiskOfFail.Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

namespace RiskOfFail.Core.Utilities
{
	public abstract class State
	{
		public StateMachine stateMachine { protected get; set; }

		public virtual void OnEnter() {}
		public virtual void OnExit() {}
		public virtual void OnUpdate() {}
		public virtual void OnFixedUpdate() {}

		public virtual void OnInput(InputAction.CallbackContext ctx) {}
	}
}