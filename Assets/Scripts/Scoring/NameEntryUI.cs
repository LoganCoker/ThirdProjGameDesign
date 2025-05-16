using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameEntryUI : MonoBehaviour
{
    public TextMeshProUGUI[] letterSlots;
    public TextMeshProUGUI timerText;    

    private char[] currentLetters = new char[3] { 'A', 'A', 'A' };
    private int currentIndex = 0;
    private float timeRemaining = 30f;
    private bool hasSubmitted = false;

    public int scoreToSubmit = Game.GetFinalScore();

    void Start()
    {
        if (timerText == null)
        {
            var go = GameObject.Find("TimerText");
            if (go != null)
                timerText = go.GetComponent<TextMeshProUGUI>();
            if (timerText == null)
                Debug.LogWarning("TimerText not assigned or found in scene.");
        }

        if (timerText != null)
            timerText.text = $"Time: {Mathf.Ceil(timeRemaining)}";

        UpdateLetterUI();
    }

    void Update()
    {
        if (hasSubmitted)
            return;

        // navigate letters
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentIndex = (currentIndex + 2) % 3;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            currentIndex = (currentIndex + 1) % 3;

        // change letter
        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentLetters[currentIndex] = NextChar(currentLetters[currentIndex]);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentLetters[currentIndex] = PrevChar(currentLetters[currentIndex]);

        // update timer
        timeRemaining -= Time.deltaTime;
        if (timerText != null)
            timerText.text = FormatTime(timeRemaining);

        if (timeRemaining <= 0f)
        {
            Debug.Log("Timer reached zero, invoking ConfirmName().");
            ConfirmName();
        }

        UpdateLetterUI();
    }

    private void UpdateLetterUI()
    {
        for (int i = 0; i < letterSlots.Length; i++)
        {
            letterSlots[i].text = currentLetters[i].ToString();
            letterSlots[i].color = (i == currentIndex) ? Color.yellow : Color.white;
        }
    }

    private char NextChar(char c)
    {
        return (char)(((c - 'A' + 1) % 26) + 'A');
    }

    private char PrevChar(char c)
    {
        return (char)(((c - 'A' + 25) % 26) + 'A');
    }

    private string FormatTime(float t)
    {
        t = Mathf.Max(t, 0f);
        int minutes = Mathf.FloorToInt(t / 60);
        int seconds = Mathf.FloorToInt(t % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void ConfirmName()
    {
        if (hasSubmitted)
            return;

        hasSubmitted = true;
        string playerName = new string(currentLetters);
        HighScoreManager.Instance.SubmitScore(scoreToSubmit, playerName);
        Debug.Log($"Auto-submitted score {scoreToSubmit} for {playerName}");

        gameObject.SetActive(false); // Hide UI after submission
    }
}

