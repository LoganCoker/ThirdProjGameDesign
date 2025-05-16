using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance;

    [Header("Page Counter UI")]
    public Text counterText;
    public int totalPages = 5;

    [Header("Note Panel UI")]
    public GameObject notePanel;
    public Text noteText;

    private bool[] collectedPages;
    private int count = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        collectedPages = new bool[totalPages + 1]; 
        UpdateCounterUI();
        notePanel.SetActive(false);
    }

    public void CollectPage(int id, string text)
    {
        if (id < 1 || id > totalPages) return;
        if (!collectedPages[id])
        {
            collectedPages[id] = true;
            count++;
            UpdateCounterUI();
            ShowNote(text);
        }
    }

    private void UpdateCounterUI()
    {
        counterText.text = string.Format("{0}/{1}", count, totalPages);
    }

    public void ShowNote(string text)
    {
        noteText.text = text;
        notePanel.SetActive(true);
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
    }
}

