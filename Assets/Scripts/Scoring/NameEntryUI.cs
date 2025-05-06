using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameEntryUI : MonoBehaviour
{
    public TextMeshProUGUI[] letterSlots;  
    public TextMeshProUGUI submitText;     
    public Button confirmButton;           

    private char[] currentLetters = new char[3] { 'A', 'A', 'A' };
    private int currentIndex = 0;

    [Tooltip("Score to submit when name is confirmed")]
    public int scoreToSubmit = 0;

    void Start()
    {
        UpdateLetterUI();
        confirmButton.onClick.AddListener(ConfirmName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentIndex = (currentIndex + 2) % 3;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            currentIndex = (currentIndex + 1) % 3;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentLetters[currentIndex] = NextChar(currentLetters[currentIndex]);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentLetters[currentIndex] = PrevChar(currentLetters[currentIndex]);

        UpdateLetterUI();
    }

    private void UpdateLetterUI()
    {
        for (int i = 0; i < letterSlots.Length; i++)
        {
            letterSlots[i].text = currentLetters[i].ToString();
            letterSlots[i].color = (i == currentIndex) ? Color.yellow : Color.white;
        }

        if (submitText != null)
            submitText.text = "Confirm Your Name";
    }

    private char NextChar(char c)
    {
        return (char)(((c - 'A' + 1) % 26) + 'A');
    }

    private char PrevChar(char c)
    {
        return (char)(((c - 'A' + 25) % 26) + 'A');
    }

    public void ConfirmName()
    {
        string playerName = new string(currentLetters);
        HighScoreManager.Instance.SubmitScore(scoreToSubmit, playerName);
        Debug.Log($"Submitted score {scoreToSubmit} for {playerName}");

        gameObject.SetActive(false); // Hide UI after submission
    }
}
