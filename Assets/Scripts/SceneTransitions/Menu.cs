using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject menu;

    private InputController input;

    void Start() {
        input = Game.Input;
        input.Player.Enable();
        input.UI.Disable();
        menu.SetActive(false);
    }

    void Update() {
        if (input.Player.Pause.WasPressedThisFrame()) {
            Pause();
        }

        if (input.UI.Resume.WasPressedThisFrame()) {
            Resume();
        }
    }

    public void Pause() {
        input.Player.Disable();
        input.UI.Enable();
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume() {
        input.UI.Disable();
        input.Player.Enable();
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
