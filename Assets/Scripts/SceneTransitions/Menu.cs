using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject menu;

    private InputController input;
    private PlayerInput playerInput;


    void Start() {
        input = Game.Input;
        playerInput = Game.PlayerInput;
        input.UI.Disable();
        menu.SetActive(false);
    }

    void Update() {
        if (playerInput.Pause) {
            Pause();
            playerInput.Pause = false;
        }

        if (input.UI.Resume.WasPressedThisFrame()) {
           Resume();
        }
    }

    public void Pause() {
        Game.PauseGame();
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume() {
        Game.ResumeGame();
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
