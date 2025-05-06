using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighScore;

public class HighScoreManager : MonoBehaviour
{
    [Header("High Score Config")]
    public string gameName = "GetItWhiteboi";

    [HideInInspector]
    public string playerName = "AAA";
    public static HighScoreManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitHighScoreSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitHighScoreSystem()
    {
        Debug.Log($"[HighScoreManager] Initializing with game name: {gameName}");
        HS.Init(this, gameName);
    }

    public void SubmitScore(int score)
    {
        Debug.Log($"[HighScoreManager] Submitting score {score} for {playerName}");
        HS.SubmitHighScore(this, playerName, score);
    }

    public void SubmitScore(int score, string name)
    {
        playerName = name;
        SubmitScore(score);
    }
}


