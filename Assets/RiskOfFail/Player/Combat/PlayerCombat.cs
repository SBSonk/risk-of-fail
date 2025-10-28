using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RiskOfFail.Player.Combat
{
    public class PlayerCombat : MonoBehaviour
    {
        public Transform gunBarrel;

        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.onActionTriggered += OnActionTriggered;
        }

        private void OnActionTriggered(InputAction.CallbackContext ctx)
        {
            //ctx.action.name
        }
    }
}
