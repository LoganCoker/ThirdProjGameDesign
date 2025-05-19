using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance { get; private set; }

    public int numOfPages;

    public GameObject[] pageOverlays;

    private bool[] collectedPages;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        collectedPages = new bool[numOfPages];

        for (int i = 0; i < pageOverlays.Length; i++)
        {
            if (pageOverlays[i] != null)
                pageOverlays[i].SetActive(false);
        }
    }

    public void CollectPage(int id)
    {
        if (id < 0 || id >= numOfPages) return;

        // mark and show UI
        if (!collectedPages[id])
        {
            collectedPages[id] = true;

            if (pageOverlays[id] != null)
                pageOverlays[id].SetActive(true);

            Debug.Log($"PageUIManager: showing page overlay {id}");
        }
    }

    public int TotalCollected() {
        int total = 0;
        foreach (bool page in collectedPages) {
            if (page) total++;
        }
        return total;
    }
}

