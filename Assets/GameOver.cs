using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

    void Start() {
        Time.timeScale = 0;
        Game.Input.Disable();
    }

    public void Quit() {
        Application.Quit();
    }

    public void Restart() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}