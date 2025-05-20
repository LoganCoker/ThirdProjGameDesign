using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            playerInput.Pause = false;
            Pause();
        }

        if (input.UI.Resume.WasPressedThisFrame()) {
           Resume();
        }
    }

    public void Pause() {
        Game.PauseGame();
        menu.SetActive(true);
        
    }

    public void Resume() {
        Game.ResumeGame();
        menu.SetActive(false);
        
    }

     public void Home() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
     }
}
