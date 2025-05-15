using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class Game : MonoBehaviour {

    public static InputController Input { get; private set; }
    public static Game Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }
    public static bool Paused { get; private set; }

    public static int Score { get; private set; }
    private float startTime;
    private const float maxTime = 8 * 60f;

    void Awake() {
        Instance = this;
        Input = new InputController();
        PlayerInput = GetComponent<PlayerInput>();
        DontDestroyOnLoad(this);

        Score = 0;
        startTime = Time.time;

        // testing
        Input.Enable();

        HighScoreManager.Instance.InitHighScoreSystem();
    }

    public static void PauseGame() {
        Paused = true;
        Input.UI.Enable();
        PlayerInput.enabled = false;
    }

    public static void ResumeGame() {
        Paused = false;
        Input.UI.Disable();
        PlayerInput.enabled = true;
    }

    public static void DisablePlayerControls() {
        PlayerInput.enabled = false;
    }
    
    public static void EnablePlayerControls() {
        PlayerInput.enabled = true;
    }

    public static void AddScore(int amount = 50)
    {  
        Score += amount;
        Debug.Log($"Score + 50 $");
    }

    public static int GetFinalScore()
    {
        float elapsed = Time.time - Instance.startTime;
        float ratio = Mathf.Clamp01(elapsed/ maxTime);
        float multiplier = 1f + ratio;
        int finalScore = Mathf.RoundToInt(Score * multiplier);
        return finalScore;
    }
}
