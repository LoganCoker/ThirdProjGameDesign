using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerInput : MonoBehaviour, InputController.IPlayerActions {

    #region InputVars
    public Vector2 MoveInput { get; private set; }
    public bool Sprinting { get; private set; }
    public bool Jumped { get; private set; }
    public bool Dance { get; private set; }
    public bool Attack { get; private set; }
    public bool Interacted { get; private set; }
    public bool Pause { get; set; }
    
    #endregion

    private void OnEnable() {
        Game.Input.Player.Enable();
        Game.Input.Player.SetCallbacks(this);
    }

    private void OnDisable() {
        Game.Input.Player.Disable();
        Game.Input.Player.RemoveCallbacks(this);
    }

    private void LateUpdate() {
        Jumped = false;
        Attack = false;
        Interacted = false;
    }

    #region InputReturns
    public void OnMovement(InputAction.CallbackContext context) {
        MoveInput = context.ReadValue<Vector2>();
        if (context.canceled) {
            Sprinting = false;
        }
    }
    
    public void OnSprint(InputAction.CallbackContext context) {
        if (context.performed) {
            Sprinting = true;
        } else if (context.canceled) {
            Sprinting = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (!context.performed) { return; }

        Jumped = true;
    }

    public void OnInteractable(InputAction.CallbackContext context) {
        if (!context.performed) { return; }

        Interacted = true;
    }

    public void OnHitIt(InputAction.CallbackContext context) {
        if (context.performed) {
            Dance = true;
        } else if (context.canceled) {
            Dance = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (!context.performed) { return; }

        Attack = true;
    }

    public void OnPause(InputAction.CallbackContext context) {
        if (context.performed && !Pause) {
            Pause = true;
        }
    }
    #endregion
}
