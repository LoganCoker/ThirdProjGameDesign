using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class Game : MonoBehaviour {

    public static InputController Input { get; private set; }
    public static Game Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }
    public static bool Paused { get; private set; }

    void Awake() {
        Instance = this;
        Input = new InputController();
        PlayerInput = GetComponent<PlayerInput>();

        // testing
        Input.Enable();
    }

    public static void PauseGame() {
        Paused = true;
        Input.UI.Enable();
        Input.Player.Disable();
    }

    public static void ResumeGame() {
        Paused = false;
        Input.UI.Disable();
        Input.Player.Enable();
    }
}
