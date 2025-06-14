using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Labyrinth.Character {
    public class InputManager : MonoBehaviour {

        InputActions inputActions;

        public bool isStopping;
        public bool isTurning;
        public bool isUIPressed;
        public bool isUIWasPressed;
        public bool isUIHeld;

        protected void OnEnable() {
            inputActions = new InputActions();
            inputActions.Player.Stop.started += StartedStopping;
            inputActions.Player.Stop.canceled += CanceledStopping;
            inputActions.Player.Turn.performed += PerformedTurning;
            inputActions.Player.Turn.canceled += CanceledTurning;
            inputActions.Enable();
        }

        protected void OnDisable() {
            inputActions.Disable();
            inputActions.Player.Stop.started -= StartedStopping;
            inputActions.Player.Stop.canceled -= CanceledStopping;
            inputActions.Player.Turn.performed -= PerformedTurning;
            inputActions.Player.Turn.canceled -= CanceledTurning;
        }

        void StartedStopping(InputAction.CallbackContext context) {
            isStopping = true;
        }

        void CanceledStopping(InputAction.CallbackContext context) {
            isStopping = false;
            isTurning = false;
        }

        void PerformedTurning(InputAction.CallbackContext context) {
            isTurning = true;
        }

        void CanceledTurning(InputAction.CallbackContext context) {
            isTurning = false;
        }
    }
}