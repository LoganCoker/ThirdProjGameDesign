using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class Game : MonoBehaviour {

    public static InputController Input { get; private set; }
    public static Game Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }
    public static bool Paused { get; private set; }
    public static int Score { get; private set; }
    public static float StartTime { get; private set; }
    public static bool[] Notes { get; set; }


    private const float maxTime = 8 * 60f;

    void Awake() {
        Instance = this;
        Input = new InputController();
        PlayerInput = GetComponent<PlayerInput>();
        DontDestroyOnLoad(this);

        Score = 0;

        // testing
        Input.Enable();
        Input.UI.Disable();
        Input.Player.Disable();
        Input.PlayerOther.Disable();

        HighScoreManager.Instance.InitHighScoreSystem();
    }

    public static void StartGame() {
        StartTime = Time.time;
        Input.Player.Enable();
        Input.PlayerOther.Enable();
        Score = 0;
        Notes = new bool[4];
    }

    public static void PauseGame() {
        Paused = true;
        Input.UI.Enable();
        PlayerInput.enabled = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void ResumeGame() {
        Paused = false;
        Input.UI.Disable();
        PlayerInput.enabled = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void AddScore(int amount = 50)
    {  
        Score += amount;
        Debug.Log($"Score + 50 $");
    }

    public static int GetFinalScore()
    {
        // time score multipier
        float elapsed = Time.time - StartTime;
        float ratio = Mathf.Clamp01(elapsed/ maxTime);
        float multiplier = 1f + ratio;
        int finalScore = Mathf.RoundToInt(Score * multiplier);

        // add score based on number of collectables collected
        foreach (bool page in Notes) {
            if (page) finalScore += 1500;
        }

        return finalScore;
    }
}
