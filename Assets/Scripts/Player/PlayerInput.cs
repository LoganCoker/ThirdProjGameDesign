using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, InputController.IPlayerTestActions {

    public InputController Controller {  get; private set; }
    #region InputVars
    public Vector2 MoveInput { get; private set; }
    public bool Sprinting { get; private set; }
    public bool Jumped { get; private set; }
    #endregion

    private void OnEnable() {
        Controller = new InputController();
        Controller.Enable();

        Controller.PlayerTest.Enable();
        Controller.PlayerTest.SetCallbacks(this);
    }

    private void OnDisable() {
        Controller.PlayerTest.Disable();
        Controller.PlayerTest.RemoveCallbacks(this);
    }

    private void LateUpdate() {
        Jumped = false;
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
    #endregion
}
