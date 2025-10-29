using RiskOfFail.Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RiskOfFail.Player.Movement
{
    public class PlayerMovementState : State
    {
        public PlayerFsmMovement player { protected get; set; }
        
        public PlayerInput playerInput { protected get; set; }
    }
}